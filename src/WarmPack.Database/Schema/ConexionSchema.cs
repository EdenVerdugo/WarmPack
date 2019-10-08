using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WarmPack.Extensions;

namespace WarmPack.Database.Schema
{
    public class ConexionSchema
    {
        private Conexion _conexion;        
        public string ConectionString { get; set; }
        public ConexionType ConexionType { get; set; }
        public ConexionSchema(Conexion conexion, ConexionType conexionType)
        {
            
            _conexion = conexion;
            ConexionType = conexionType;
        }

        public List<ConexionSchemaTable> SchemaTables()
        {
            return SchemaTables("", false);
        }

        public List<ConexionSchemaTable> SchemaTables(bool withColumns)
        {
            return SchemaTables("", withColumns);
        }

        public List<ConexionSchemaTable> SchemaTables(string tableName)
        {
            return SchemaTables(tableName, false);
        }

        public List<ConexionSchemaTable> SchemaTables(string tableName, bool withColumns = false)
        {
            List<ConexionSchemaTable> tables = new List<ConexionSchemaTable>();
            
            switch (ConexionType)
            {
                case ConexionType.MSSQLServer:
                    var sql = $@"SELECT TABLE_NAME Name FROM { _conexion._conexion.Database }.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = { (string.IsNullOrEmpty(tableName) ? "TABLE_NAME" : string.Format("'{0}'", tableName)) } ORDER BY TABLE_NAME";

                    var r = _conexion.ExecuteWithResults<ConexionSchemaTable>(sql, out tables);

                    if (withColumns)
                        tables.Map(t => 
                        {
                            t.Columns = SchemaColumns(t.Name);
                            return t;                           
                        });

                    break;
                    //case ConexionType.PostgreSQL:

                    //    break;

            }

            return tables;
        }

        public List<ConexionSchemaColumn> SchemaColumns(string tableName)
        {
            List<ConexionSchemaColumn> columns = new List<ConexionSchemaColumn>();

            switch (ConexionType)
            {
                case ConexionType.MSSQLServer:
                    var sql = $@"
SELECT	c.COLUMN_NAME Name, 
		c.ORDINAL_POSITION OrdinalPosition, 
		CAST(CASE when c.IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS BIT) IsNullable, 
		c.DATA_TYPE DataType, 
		CASE WHEN c.DATA_TYPE LIKE '%char' THEN c.CHARACTER_MAXIMUM_LENGTH ELSE c.NUMERIC_PRECISION END Length, 
		c.NUMERIC_SCALE Scale 
FROM { _conexion._conexion.Database }.INFORMATION_SCHEMA.COLUMNS c 
WHERE c.TABLE_NAME = '{tableName}' 
ORDER BY c.ORDINAL_POSITION
";
                    var r = _conexion.ExecuteWithResults<ConexionSchemaColumn>(sql, out columns);

                    break;
                    //case ConexionType.PostgreSQL:

                    //    break;

            }

            return columns;
        }

        public class ConexionSchemaTable
        {
            public string Name { get; set; }
            public List<ConexionSchemaColumn> Columns { get; set; }
        }

        public class ConexionSchemaColumn
        {
            public string Name { get; set; }
            public int OrdinalPosition { get; set; }
            public bool IsNullable { get; set; }
            public string DataType { get; set; }
            public int Length { get; set; }
            public int Scale { get; set; }
        }
    }
}
