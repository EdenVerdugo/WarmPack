using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarmPack.Extensions;

namespace Warmpack.Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 0;

            try
            {
                int y = 1 / x;
            }
            catch(Exception ex)
            {
                ex.Log();
            }

        }
    }
}
