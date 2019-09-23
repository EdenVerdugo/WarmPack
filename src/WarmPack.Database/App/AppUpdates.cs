using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Helpers;

namespace WarmPack.Database.App
{
    public static class AppUpdates
    {
        public static void ApplySQL(Conexion conexion, string path)
        {
            var di = new DirectoryInfo(path);
            var rgFiles = di.GetFiles("*.sql");

            foreach (var fi in rgFiles)
            {
                var fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();

                conexion.ExecuteScript(script);                
            }
        }

        public static void ApplySQL(string connectionString, string path)
        {
            var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);
            var di = new DirectoryInfo(path);
            var rgFiles = di.GetFiles("*.sql");

            foreach (var fi in rgFiles)
            {
                var fileInfo = new FileInfo(fi.FullName);
                string script = fileInfo.OpenText().ReadToEnd();

                conexion.ExecuteScript(script);
            }
        }

        public static void ApplySQL(string connectionString, string[] files)
        {
            var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);
                        
            var filterFiles = files.Where(x => x.EndsWith(".sql"));

            foreach (var fi in filterFiles)
            {
                var fileInfo = new FileInfo(fi);
                string script = fileInfo.OpenText().ReadToEnd();

                conexion.ExecuteScript(script);
            }
        }

        public static void ApplySQLEmbeddedResource(string connectionString, string[] resourceNames = null)
        {
            var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);

            var assembly = Assembly.GetCallingAssembly();

            if (resourceNames == null)
                resourceNames = assembly.GetManifestResourceNames();            

            foreach (var resourceName in resourceNames)
            {                
                if(resourceName.Split('.').LastOrDefault() == "sql")
                {
                    using (var ms = assembly.GetManifestResourceStream(resourceName))
                    {
                        using (var sr = new StreamReader(ms))
                        {
                            var script = sr.ReadToEnd();

                            var result = conexion.ExecuteScript(script);

                            var log = new WarmPack.Utilities.Log($"{WarmPack.App.Globals.ApplicationPath}//Updates.log");

                            foreach(var r in result)
                            {
                                log.WriteLog($"{resourceName} - {r.Message}");
                            }

                        }
                    }                    
                    
                }
            }            
        }
    }
}
