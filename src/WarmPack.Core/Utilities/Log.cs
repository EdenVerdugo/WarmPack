using System;
using System.IO;
using WarmPack.App;
using WarmPack.Extensions;
using WarmPack.Helpers;

namespace WarmPack.Utilities
{
    public class Log
    {
        //public static readonly string LogPath = @"C:\Logs\" + Globals.ApplicationName + ".log";
        private string _Path = ""; 

        private static string GetLogName(string directory = "")
        {
            if (string.IsNullOrEmpty(directory))
            {
                directory = $@"C:\Logs\{ Globals.ApplicationName }\{ DateTime.Now.ToString("yyyy")}\{ DateTime.Now.ToString("MM")}";                
            }

            DirectoryHelper.CreateDirectories(directory);

            var fileName = $@"[{ DateTime.Now.ToString("yyyy-MM-dd") }] {Globals.ApplicationName}.log";            

            return Path.Combine(directory, fileName);            
        }

        public Log()
        {

        }

        public Log(string path)
        {
            _Path = path;
        }

        public void WriteLog(string message)
        {
            _Write(message, _Path);
        }        

        public static void Write(string message)
        {
            _Write(message, GetLogName());            
        }

        public static void Write(string message, string saveDirectory)
        {
            _Write(message, GetLogName(saveDirectory));
        }


        private static void _Write(string message, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = GetLogName();
                }

                var fileInfo = new FileInfo(path);

                var directory = new DirectoryInfo(fileInfo.DirectoryName);

                foreach (var dir in directory.GetDirectoryPaths())
                {
                    if (!Directory.Exists(dir.FullName))
                        Directory.CreateDirectory(dir.FullName);
                }

                string resto = "";
                if (System.IO.File.Exists(path))
                {
                    System.IO.StreamReader lector = new System.IO.StreamReader(path);
                    resto = lector.ReadToEnd();

                    lector.Close();
                    lector.Dispose();
                }

                System.IO.StreamWriter escritor = new System.IO.StreamWriter(path);

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
