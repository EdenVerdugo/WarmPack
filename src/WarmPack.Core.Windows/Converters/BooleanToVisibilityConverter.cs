using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WarmPack.Windows.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility resultado;

            resultado = ((bool)value) ? Visibility.Visible : Visibility.Collapsed;

            return resultado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
