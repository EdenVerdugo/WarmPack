using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Core.Helpers;
using WarmPack.Data;

namespace WarmPack.Database
{
    public class ConexionTools
    {
        private readonly SqlConnection sqlConnection = null;

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
            sqlConnection = new SqlConnection(this.ConnectionString.ToMsqlConnectionString());
        }

        public List<string> Databases()
        {
            var databases = new List<string>();

            string query = @"
SELECT d.name
FROM sys.databases d
WHERE d.name NOT IN (
'master',
'tempdb',
'model',
'msdb')
ORDER BY d.name";

            _conexion.ExecuteWithResults(query, row =>
            {
                var db = row["name"].ToString();

                databases.Add(db);
            });

            return databases;
        }

        private object ExecuteScript(string script, bool withResults)
        {
            try
            {
                using (var conexion = new SqlConnection(this.ConnectionString.ToMsqlConnectionString()))
                {
                    var server = new Server(new ServerConnection(conexion));

                    if (!withResults)
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

                if (ex.HResult == -2146232799)
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
            try
            {
                var script = new System.IO.FileInfo(fileName).OpenText().ReadToEnd();

                var result = ExecuteScript(fileName, false) as int?;

                return result.Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
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

            string query = $@"
USE master;

GO

ALTER DATABASE {dbName} 
SET RECOVERY SIMPLE;
GO

BACKUP DATABASE {dbName} 
TO DISK = N'{ backupFileName }'    
WITH NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD, 
STATS = 10

GO

ALTER DATABASE {dbName} 
SET RECOVERY FULL;
GO

";            

            ExecuteScript(query, false);            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName">Nombre de la base de datos</param>
        /// <param name="srcBak">Ruta del archivo .bak</param>
        /// <param name="mdf">Ruta para el archivo de la base de datos (mdf)</param>
        /// <param name="ldf">Ruta para el archivo log (ldf)</param>
        public void RestoreDatabaseBackup(string dbName, string srcBak, string mdfName, string ldfName)
        {
            string guid = Guid.NewGuid().ToString();


            //var query = $@"USE MASTER RESTORE DATABASE {dbName} FROM DISK = '{srcBak}' WITH RECOVERY, MOVE '{dbName}' TO '{mdf}', MOVE '{dbName}_log' TO '{ldf}', REPLACE";
            //_conexion.Execute(query);

            var srcDb = _conexion.ExecuteScalar("SELECT SERVERPROPERTY('InstanceDefaultDataPath')").ToString();


            string dbNameCopia = dbName + "Copia" + DateTime.Now.ToString("yyyyMMddhhmmss");
            string dbNameBak = dbName + "Backup" + DateTime.Now.ToString("yyyyMMddhhmmss");
            var script = $@"
USE MASTER

GO

CREATE DATABASE {dbNameCopia}
ON
(NAME = '{mdfName}',FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.mdf')
LOG ON
(NAME = '{ldfName}',FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.ldf') 
	
GO

RESTORE DATABASE {dbNameCopia} 
FROM DISK = '{srcBak}' 
WITH RECOVERY, 
MOVE '{dbName}' TO 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.mdf', 
MOVE '{dbName}_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.ldf',
REPLACE

IF EXISTS(SELECT * FROM sys.databases d WHERE d.name = '{dbName}')
BEGIN
	
    ALTER DATABASE { dbName } 
    SET SINGLE_USER 
    WITH ROLLBACK IMMEDIATE    

	EXEC sys.sp_renamedb '{ dbName }', '{ dbNameBak }'

    ALTER DATABASE { dbNameBak } 
    SET MULTI_USER 
END

GO


ALTER DATABASE { dbNameCopia } 
SET SINGLE_USER 
WITH ROLLBACK IMMEDIATE    

EXEC sys.sp_renamedb '{ dbNameCopia }', '{ dbName }'

ALTER DATABASE { dbName } 
SET MULTI_USER 

";
            ExecuteScript(script, false);

        }

        public void RestoreDatabaseBackupEx(string dbName, string srcBackup)
        {
            string guid = Guid.NewGuid().ToString();

            var server = new Server(new ServerConnection(sqlConnection));

            Restore restoreDB = new Restore();
            restoreDB.Database = dbName;
            // Specify whether you want to restore database, files or log
            restoreDB.Action = RestoreActionType.Database;
            restoreDB.Devices.AddDevice(srcBackup, DeviceType.File);

            //server.DefaultFile                

            var fs = restoreDB.ReadFileList(server);

            RelocateFile mdf = new RelocateFile();
            mdf.LogicalFileName = fs.Rows[0]["LogicalName"].ToString();
            mdf.PhysicalFileName = Path.Combine(server.DefaultFile, $@"{dbName}-{guid}.mdf");  //fs.Rows[0]["PhysicalName"].ToString();

            RelocateFile ldf = new RelocateFile();
            ldf.LogicalFileName = fs.Rows[1]["LogicalName"].ToString();
            ldf.PhysicalFileName = Path.Combine(server.DefaultLog, $@"{dbName}-{guid}.ldf");  //fs.Rows[1]["PhysicalName"].ToString();

            restoreDB.RelocateFiles.Add(mdf);
            restoreDB.RelocateFiles.Add(ldf);

            restoreDB.ReplaceDatabase = true; // will overwrite any existing DB     
            restoreDB.NoRecovery = false;

            // you can Wire up events for progress monitoring */
            restoreDB.PercentComplete += RestoreDB_PercentComplete;
            restoreDB.Complete += RestoreDB_Complete;

            restoreDB.SqlRestore(server);
        }

        private void RestoreDB_Complete(object sender, ServerMessageEventArgs e)
        {
            var status = e;
        }

        private void RestoreDB_PercentComplete(object sender, PercentCompleteEventArgs e)
        {
            var status = e;
        }
    }
}
