using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Extensions
{
    public static class ArrayExtensions
    {
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (var item in array)
            {
                action?.Invoke(item);
            }
        }
    }
}
