using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WarmPack.Extensions;

namespace WarmPack.Windows.Converters
{
    public class EmptyStringToZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object val = null;

            if (value is string && (
                targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(decimal) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(short) ||
                targetType == typeof(byte) ||
                targetType == typeof(uint) ||
                targetType == typeof(ulong) ||
                targetType == typeof(ushort)))
            {


                //val = 0;                                
                if (value.ToString().IsNullOrWhiteSpace())
                {
                    val = 0;
                }
                else
                {
                    val = value;
                }
            }

            return val;
        }
    }
}
