﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WarmPack.Windows.Controls;

namespace WarmPack.Windows.Converters
{
    public class CommandParameterToCommandParameterOnKeyEnter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CommandParameterOnKeyDown resultado = new CommandParameterOnKeyDown()
            {
                CommandParameter = value
            };

            return resultado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
