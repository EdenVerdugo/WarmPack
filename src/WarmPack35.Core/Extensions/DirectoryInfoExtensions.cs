using System;
using System.Collections.Generic;
using System.IO;

namespace WarmPack.Extensions
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Obtiene la lista de directorios que se desprenden de un directorio dado.        
        /// </summary>
        /// <param name="path">Es el directorio del que se desea obtener todas sus ramificaciones.</param>
        /// <returns>
        /// Ejemplo :
        /// C:\Dir1\Dir2\Dir3
        /// 
        /// Regresa :
        /// C:\
        /// C:\Dir1
        /// C:\Dir1\Dir2
        /// C:\Dir1\Dir2\Dir3
        /// </returns>
        public static List<DirectoryInfo> GetDirectoryPaths(this DirectoryInfo path)
        {
            if (path == null)
                throw new ArgumentNullException("No se ha especificado un path válido.");

            var ret = new List<DirectoryInfo>();
            if (path.Parent != null)
                ret.AddRange(GetDirectoryPaths(path.Parent));

            ret.Add(path);

            return ret;
        }

        /// <summary>
        /// Obtiene la lista de nombres de los directorios que se desprenden de un directorio dado.
        /// </summary>
        /// <param name="path">Es el directorio del que se desea obtener todos los nombres de sus carpetas.</param>
        /// <returns>
        /// Ejemplo:
        /// C:\Dir1\Dir2\Dir3
        /// 
        /// Regresa:
        /// C:\
        /// Dir1
        /// Dir2
        /// Dir3
        /// </returns>
        public static List<string> GetDirectoryNames(this DirectoryInfo path)
        {
            if (path == null)
                throw new ArgumentNullException("No se ha especificado un path válido.");

            var ret = new List<string>();
            if (path.Parent != null)
                ret.AddRange(GetDirectoryNames(path.Parent));

            ret.Add(path.Name);

            return ret;
        }
    }
}
