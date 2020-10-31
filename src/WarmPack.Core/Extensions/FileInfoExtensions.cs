using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Core.Helpers;

namespace WarmPack.Core.Extensions
{
    public static class FileInfoExtensions
    {        

        public static string GetMimeType(this FileInfo file)
        {
            return FileHelper.GetMimeTypeByExtension(file.Extension);            
        }
    }
}
