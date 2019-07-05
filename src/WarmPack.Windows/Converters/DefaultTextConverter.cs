using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WarmPack.Windows.Converters
{
    public class DefaultTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object resultado;

            if (targetType == typeof(int))
            {
                resultado = ((int)value) == 0 ? "" : value;
            }
            else if (targetType == typeof(decimal))
            {
                resultado = ((decimal)value) == 0 ? "" : value;
            }
            else if (targetType == typeof(float))
            {
                resultado = ((float)value) == 0 ? "" : value;
            }
            else if (targetType == typeof(long))
            {
                resultado = ((long)value) == 0 ? "" : value;
            }
            else if (targetType == typeof(double))
            {
                resultado = ((double)value) == 0 ? "" : value;
            }
            else
            {
                resultado = value;
            }

            return resultado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
