using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Staffinfo.Desktop.Model;

namespace Staffinfo.Desktop.Converters
{
    public class NewConverters : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var selectitem = value as ServiceModel;
            var serID = Param as PostModel;

            if (selectitem == null || serID == null)
                return Visibility.Collapsed;

            return selectitem.Id == serID.ServiceId ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static DependencyProperty ParamProperty = DependencyProperty.Register("Param", typeof(object), typeof(NewConverters), new PropertyMetadata(null, null) );

        public object Param
        {
            get { return GetValue(ParamProperty); }
            set {  SetValue(ParamProperty, value);}
        }
    }

}
