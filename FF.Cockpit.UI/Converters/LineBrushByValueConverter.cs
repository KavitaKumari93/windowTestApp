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
    public class LineBrushByValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = Brushes.Transparent;

            int val;
            bool isValueParse = Int32.TryParse(value.ToString(), out val);

            if (isValueParse)
            {
                if (val >= 90)
                    brush = (Brush)new BrushConverter().ConvertFromString("#F30D14");
                else if (val <= 40)
                    brush = (Brush)new BrushConverter().ConvertFromString("#199258");
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
