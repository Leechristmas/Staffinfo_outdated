using System;
using System.Globalization;
using System.Windows.Data;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Converters
{
    public class StringToIntConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int val = 0;
            var stringValue = value as string;
            if (!Int32.TryParse(stringValue, out val)) return null;

            return value;
        }
    }
}