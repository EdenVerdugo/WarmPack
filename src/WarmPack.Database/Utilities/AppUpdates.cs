using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //using (var connection = new SqlConnection(sqlConnectionString))
                //{
                //    Server server = new Server(new ServerConnection(connection));
                //    server.ConnectionContext.ExecuteNonQuery(script);
                //}
            }

        }
    }
}
