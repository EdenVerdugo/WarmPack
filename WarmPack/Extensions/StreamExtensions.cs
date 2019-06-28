using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Extensions
{
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] ToArray(this Stream src)
        {
            MemoryStream ms = new MemoryStream();
            CopyTo(src, ms);

            return ms.ToArray();

            /*
            //deprecated parece ser que muchas veces el length que muestra en stream.length puede no ser el tamaño correcto
            //fuente : https://stackoverflow.com/questions/221925/creating-a-byte-array-from-a-stream
            byte[] bytes = new byte[src.Length];

            src.Read(bytes, 0, bytes.Length);

            return bytes;
            */
        }

    }
}
