using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Core.Helpers;
using WarmPack.Data;

namespace WarmPack.Database
{
    public class ConexionTools
    {
        private readonly Conexion _conexion = null;

        private ConnectionString ConnectionString
        {
            get
            {
                return _conexion.ConnectionString;
            }
        }


        public ConexionTools(Conexion conexion)
        {
            _conexion = conexion;            
        }

        private object ExecuteScript(string fileName, bool withResults)
        {
            try
            {
                var script = new System.IO.FileInfo(fileName).OpenText().ReadToEnd();

                using (var conexion = new SqlConnection(this.ConnectionString.ToMsqlConnectionString()))
                {
                    var server = new Server(new ServerConnection(conexion));

                    if (withResults)
                    {
                        return server.ConnectionContext.ExecuteNonQuery(script);
                    }
                    else
                    {
                        return server.ConnectionContext.ExecuteWithResults(script);
                    }

                }
            }
            catch (Exception ex)
            {
                string anexo = "";

                if (ex.HResult == -2146233087)
                {
                    anexo = @"
Si es un problema de incompatibilidad con el framework trata de que en el archivo .config quede de la siguiente manera:

<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
    <startup useLegacyV2RuntimeActivationPolicy = ""true"">
        <supportedRuntime version = ""v4.0"" sku = "".NETFramework,Version=v4.5.2""/>
    </startup>
</configuration> ";
                }

                throw new Exception(ex.Message + anexo, ex);
            }
        }


        public int ExecuteScriptFromFile(string fileName)
        {
            var result = ExecuteScript(fileName, false) as int?;

            return result.Value;
        }

        public DataSet ExecuteScriptWithResultsFromFile(string fileName)
        {
            var result = ExecuteScript(fileName, false) as DataSet;

            return result;
        }

        public void ExecuteBulkCopy(string destinationTable, DataTable table)
        {
            using (var copy = new SqlBulkCopy(this.ConnectionString.ToMsqlConnectionString()))
            {
                copy.DestinationTableName = destinationTable;

                copy.WriteToServer(table);
            }
        }

        public void ExecuteBulkCopy(Func<SqlBulkCopy, DataTable> function)
        {
            using (var copy = new SqlBulkCopy(this.ConnectionString.ToMsqlConnectionString()))
            {
                var dt = function(copy);

                copy.WriteToServer(dt);
            }
        }

        public void ExecuteBulkInsertFromFile(string file, string tableName, int firstRow = 3, string fieldTerminator = "|", string rowTerminator = "\n", string codePage = "ACP")
        {
            string query = $"BULK INSERT {tableName} FROM '{file}' WITH (firstrow = {firstRow}, FIELDTERMINATOR = '{fieldTerminator}', ROWTERMINATOR='{rowTerminator}', CODEPAGE = '{codePage}')";

            _conexion.Execute(query);
        }

        public void CreateDatabaseBackup(string dbName, string backupFileName)
        {
            if (System.IO.File.Exists(backupFileName))
            {
                System.IO.File.Move(backupFileName, $"{backupFileName.Replace(".bak", "").Replace(".txt","").Replace(".data","")}-{ Guid.NewGuid() }(Backup de Backup).bak");
            }

            string query = $@"BACKUP DATABASE {dbName} 
  TO DISK = N'{ backupFileName }'    
  WITH NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD, 
  STATS = 10";            

            _conexion.Execute(query);            
        }
    }
}
