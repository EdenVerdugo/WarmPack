using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.App;
using WarmPack.Database;
using WarmPack.Windows.Search;
using WarmPack.Windows.App;

namespace WarmPack.Tests
{
    class Program
    {
        class Articulo
        {
            public int CodArticulo { get; set; }
            public string Descripcion { get; set; }            
        }

        [STAThread]
        static void Main(string[] args)
        {
            AppConfiguration config = new AppConfiguration(@"C:\Sid\Config.xml", new WarmPack.Utilities.Encrypter("Difa123mer"));

            var cadenaConexion = config.TryConnectionString("ConexionAlexiña", true, true);


            var CadenaConexionPrincipal = config.ConnectionString("ConexionActualizacionesLocal", true);
            var NomEmpresa = config.Parameter("NomEmpresa").TryString(p => "");
            var DondeCheco = config.Parameter("DondeCheco").TryByte(p => 0);

            //var conexion = new Conexion(ConexionType.MSSQLServer, "data source = 172.19.1.22; initial catalog = sid; user id = sa; password = Difarmer01");

            //var buscador = new Searcher<Articulo>((buscar) =>
            //{
            //    var r = conexion.ExecuteWithResults<Articulo>("select top 10 codarticulo, descripcion from catarticulos");

            //    return r.Data;
            //});

            //buscador.AddColumnToShow(c => c.CodArticulo, SearcherTextAlignment.Center);
            //buscador.AddColumnToShow(c => c.Descripcion);

            //buscador.Search("paracetamol");


        }
    }
}
