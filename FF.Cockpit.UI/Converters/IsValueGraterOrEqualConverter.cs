using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace FF.Cockpit.UI.Converters
{
    public class IsValueGraterOrEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isValueGraterOrEqual = false;

            int val;
            int parm;

            bool isValueParse = Int32.TryParse(value.ToString(), out val);
            bool isParameterParse = Int32.TryParse(parameter.ToString(), out parm);


            if (isValueParse && isParameterParse)
                isValueGraterOrEqual = val > parm;

            return isValueGraterOrEqual;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
