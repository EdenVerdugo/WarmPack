using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Classes;
using WarmPack.Database;

namespace WarmPack.Magic.DataAccess
{
    public class ConexionSnippets 
    {
        private readonly Conexion _conexion = null;

        public ConexionSnippets(Conexion conexion)
        {
            _conexion = conexion;
        }

        public Result<List<T>> GetResults<T>(string query, Action<ConexionParameters> parameters, bool addResultMsg = true, string resultNameParam = "@pResultado", string msgNameParam = "@pMsg")
        {
            var param = new ConexionParameters();
            if (addResultMsg)
            {
                param.Add(resultNameParam, ConexionDbType.Bit, System.Data.ParameterDirection.Output);
                param.Add(msgNameParam, ConexionDbType.VarChar, System.Data.ParameterDirection.Output, 300);
            }

            parameters(param);

            var r = _conexion.ExecuteWithResults<T>(query, param);

            return r;
        }
        
    }
}
