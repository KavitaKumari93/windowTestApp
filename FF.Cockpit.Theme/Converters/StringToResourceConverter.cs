using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FF.Cockpit.Theme.Converters
{
   public class StringToResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return null;

            //var resource = Application.Current.FindResource("Icons.xaml");
            //return Application.Current.TryFindResource(value);
            return Application.Current.TryFindResource(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
