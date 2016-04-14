using System;
using System.Globalization;
using System.Windows.Data;
using Staffinfo.Desktop.Properties;

namespace Staffinfo.Desktop.Converters
{
    /// <summary>
    /// Конвертирует header'ы для data grid
    /// </summary>
    public class DataGridHeaderConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (stringValue == null) return null;


            return Resources.ResourceManager.GetString(stringValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}