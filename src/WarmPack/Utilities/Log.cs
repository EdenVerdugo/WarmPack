using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.App;
using WarmPack.Extensions;

namespace WarmPack.Utilities
{
    public static class Log
    {
        public static readonly string LogPath = @"C:\Logs\" + Globals.ApplicationName + ".log";

        public static void Write(string message)
        {
            try
            {
                var fileInfo = new FileInfo(LogPath);

                var directory = new DirectoryInfo(fileInfo.DirectoryName);

                foreach (var dir in directory.GetDirectoryPaths())
                {
                    if (!Directory.Exists(dir.FullName))
                        Directory.CreateDirectory(dir.FullName);
                }

                string resto = "";
                if (System.IO.File.Exists(LogPath))
                {
                    System.IO.StreamReader lector = new System.IO.StreamReader(LogPath);
                    resto = lector.ReadToEnd();

                    lector.Close();
                    lector.Dispose();
                }

                System.IO.StreamWriter escritor = new System.IO.StreamWriter(LogPath);

                escritor.WriteLine(string.Format("[ {0} ] {1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"), message));

                escritor.Write(resto);

                escritor.Close();
                escritor.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
