using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace FF.Cockpit.UI.Converters
{
    public class OpacityByValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacity = 0.5;

            int val;
            bool isValueParse = Int32.TryParse(value.ToString(), out val);

            if (isValueParse)
            {
                if (val >= 90)
                    opacity = 1;
                else if (val <= 40)
                    opacity = 1;
            }

            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
