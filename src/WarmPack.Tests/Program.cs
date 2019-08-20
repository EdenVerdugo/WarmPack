using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Database;
using WarmPack.Windows.Search;

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
