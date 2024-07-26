using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FF.Cockpit.Common;

namespace FF.Cockpit.UI.Converters
{
    public class EqualWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double returnValue = 0;
            if (values != null && (int)values[1] > 0)
                returnValue = (double)values[0] / (int)values[1];
            return returnValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
