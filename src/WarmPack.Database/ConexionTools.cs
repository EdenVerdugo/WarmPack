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
using System.Threading;
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


        public void DatabaseShrink(string dbName, int percent = 0, ToolsShrinkMethod shrinkMethod = ToolsShrinkMethod.TruncateOnly)
        {
            var server = new Server(new ServerConnection(sqlConnection));
            var db = server.Databases[dbName];

            if (db == null)
            {
                throw new Exception("No se encontró la base de datos");
            }

            db.Shrink(percent, (ShrinkMethod)shrinkMethod);
        }

        public void DatabaseRename(string dbName, string dbNewName)
        {
            var server = new Server(new ServerConnection(sqlConnection));
            var db = server.Databases[dbName];

            if(db == null)
            {
                throw new Exception("No se encontró la base de datos");
            }
            
            db.Rename(dbNewName);
        }

        public void DatabaseDrop(string dbName)
        {
            var server = new Server(new ServerConnection(sqlConnection));
            var db = server.Databases[dbName];

            if (db == null)
            {
                throw new Exception("No se encontró la base de datos");
            }

            db.Drop();
        }

        public void ExecuteScript(string script)
        {
            _ExecuteScript(script, false);
        }

        public DataSet ExecuteScriptWithResults(string script)
        {
            return _ExecuteScript(script, true) as DataSet;
        }

        private object _ExecuteScript(string script, bool withResults)
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

                var result = _ExecuteScript(script, false) as int?;

                return result.Value;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public DataSet ExecuteScriptWithResultsFromFile(string fileName)
        {
            var result = _ExecuteScript(fileName, true) as DataSet;

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

        public void DatabaseCreateBackup(string dbName, string backupFileName)
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

            _ExecuteScript(query, false);            
        }


//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="dbName">Nombre de la base de datos</param>
//        /// <param name="srcBak">Ruta del archivo .bak</param>
//        /// <param name="mdf">Ruta para el archivo de la base de datos (mdf)</param>
//        /// <param name="ldf">Ruta para el archivo log (ldf)</param>
//        public void RestoreDatabaseBackup(string dbName, string srcBak, string mdfName, string ldfName)
//        {
//            string guid = Guid.NewGuid().ToString();


//            //var query = $@"USE MASTER RESTORE DATABASE {dbName} FROM DISK = '{srcBak}' WITH RECOVERY, MOVE '{dbName}' TO '{mdf}', MOVE '{dbName}_log' TO '{ldf}', REPLACE";
//            //_conexion.Execute(query);

//            var srcDb = _conexion.ExecuteScalar("SELECT SERVERPROPERTY('InstanceDefaultDataPath')").ToString();


//            string dbNameCopia = dbName + "Copia" + DateTime.Now.ToString("yyyyMMddhhmmss");
//            string dbNameBak = dbName + "Backup" + DateTime.Now.ToString("yyyyMMddhhmmss");
//            var script = $@"
//USE MASTER

//GO

//CREATE DATABASE {dbNameCopia}
//ON
//(NAME = '{mdfName}',FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.mdf')
//LOG ON
//(NAME = '{ldfName}',FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.ldf') 
	
//GO

//RESTORE DATABASE {dbNameCopia} 
//FROM DISK = '{srcBak}' 
//WITH RECOVERY, 
//MOVE '{dbName}' TO 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.mdf', 
//MOVE '{dbName}_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbNameCopia}.ldf',
//REPLACE

//IF EXISTS(SELECT * FROM sys.databases d WHERE d.name = '{dbName}')
//BEGIN
	
//    ALTER DATABASE { dbName } 
//    SET SINGLE_USER 
//    WITH ROLLBACK IMMEDIATE    

//	EXEC sys.sp_renamedb '{ dbName }', '{ dbNameBak }'

//    ALTER DATABASE { dbNameBak } 
//    SET MULTI_USER 
//END

//GO


//ALTER DATABASE { dbNameCopia } 
//SET SINGLE_USER 
//WITH ROLLBACK IMMEDIATE    

//EXEC sys.sp_renamedb '{ dbNameCopia }', '{ dbName }'

//ALTER DATABASE { dbName } 
//SET MULTI_USER 

//";
//            ExecuteScript(script, false);

//        }

        public AutoResetEvent DatabaseRestoreBackup(string dbName, string srcBackup, Action<RestoreDatabaseBackupEventArgs> onPercentComplete = null, Action<SqlError> onComplete = null)
        {
            AutoResetEvent waitHandle = new AutoResetEvent(false);

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
            restoreDB.PercentComplete += (s, e) =>
            {
                onPercentComplete?.Invoke(new RestoreDatabaseBackupEventArgs(e.Error, e.Message, e.Percent));
            };

            restoreDB.Complete += (s, e) =>
            {
                onComplete?.Invoke(e.Error);

                waitHandle.Set();
            };

            restoreDB.SqlRestore(server);

            return waitHandle;
        }        
    }
}


public class RestoreDatabaseBackupEventArgs
{
    public SqlError Error { get; set; }
    public string Message { get; set; }
    public int Percent { get; set; }

    public RestoreDatabaseBackupEventArgs()
    {
        
    }

    public RestoreDatabaseBackupEventArgs(SqlError error, string message, int percent)
    {
        Error = error;
        Message = message;
        Percent = percent;
    }
}

public enum ToolsShrinkMethod
{
    Default,
    NoTruncate,
    TruncateOnly,
    EmptyFile
}