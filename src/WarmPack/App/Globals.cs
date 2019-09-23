using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WarmPack.App
{
    //       Autor: L.I. Eden Verdugo
    //       Fecha: 2018/08/10    
    // Comentarios: Aqui pondre propiedades globales para las aplicaciones
    /// <summary>
    /// Lista de propiedades globales.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Obtiene el directorio de la aplicación.
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        /// <summary>
        /// Obtiene el nombre de la aplicación.
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                return Assembly.GetEntryAssembly()?.GetName()?.Name;
            }
        }

        public static string ApplicationVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        

    }
}
