using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Utilities
{
    public static class AppUpdates
    {
        public static void ApplySQL(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    DirectoryInfo di = new DirectoryInfo(path);
            //    var sqlFiles = di.GetFiles("*.sql");

            //    foreach (var file in sqlFiles)
            //    {
            //        var fileInfo = new FileInfo(file.FullName);
            //        string script = fileInfo.OpenText().ReadToEnd();

            //        using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            //        {
            //            Server server = new Server(new ServerConnection(connection));
            //            server.ConnectionContext.ExecuteNonQuery(script);
            //        }

            //    }
            //}            
        }

    }
}
