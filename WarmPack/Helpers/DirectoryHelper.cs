using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Extensions;

namespace WarmPack.Helpers
{
    public abstract class DirectoryHelper
    {
        public static void CreateDirectories(string fullPath)
        {
            var directory = new DirectoryInfo(fullPath);

            foreach (var dir in directory.GetDirectoryPaths())
            {
                if (!Directory.Exists(dir.FullName))
                    Directory.CreateDirectory(dir.FullName);
            }
        }

        public static void Copy(string source, string dest)
        {
            Copy(source, dest, true, true);
        }

        public static void Copy(string source, string dest, bool overwrite = false)
        {
            Copy(source, dest, true, overwrite);
        }

        public static void Copy(string source, string dest, Action<FileInfo> actionForFile)
        {
            Copy(source, dest, true, false, actionForFile);
        }

        public static void Copy(string source, string dest, Action<FileInfo> actionForFile, bool overwrite = false)
        {
            Copy(source, dest, overwrite, false, actionForFile);
        }

        public static void Copy(string source, string dest, bool overwrite = true, bool deleteSource = false, Action<FileInfo> actionForFile = null)
        {
            CreateDirectories(dest);

            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), overwrite);

                actionForFile?.Invoke(new FileInfo(file));

                if (deleteSource)
                {
                    if (File.Exists(file))
                        File.Delete(file);
                }
            }

            foreach (var directory in Directory.GetDirectories(source))
            {
                Copy(directory, Path.Combine(dest, Path.GetFileName(directory)), overwrite, deleteSource, actionForFile);

                if (deleteSource)
                {
                    Directory.Delete(directory);
                }
            }
        }

        public static void Move(string source, string dest)
        {
            Copy(source, dest, true, true);
        }

        public static void Move(string source, string dest, Action<FileInfo> actionForFile)
        {
            Copy(source, dest, true, true, actionForFile);
        }
    }
}
