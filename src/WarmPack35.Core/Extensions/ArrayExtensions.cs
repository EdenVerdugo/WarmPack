using System;
using System.Collections.Generic;

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

        public static T[] Map<T>(this T[] array, Func<T, T> func)
        {
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = func(array[i]);
            }

            return array;
        }

        public static List<T> Map<T>(this List<T> array, Func<T, T> func)
        {
            for (int i = 0; i < array.Count; i++)
            {
                array[i] = func(array[i]);
            }

            return array;
        }
    }
}
