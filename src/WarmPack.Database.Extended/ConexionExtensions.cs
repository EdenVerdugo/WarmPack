using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Database
{
    public static class ConexionExtensions
    {
        public static ConexionTools Tools(this Conexion conexion)
        {
            return new ConexionTools(conexion);                        
        }
    }
}
