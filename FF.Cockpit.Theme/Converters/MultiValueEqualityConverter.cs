using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static System.Net.Mime.MediaTypeNames;

namespace FF.Cockpit.Theme.Converters
{
    public class MultiValueEqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object returnValues=null;
            if (values.Length==2)
                returnValues= values[0]?.ToString().Trim() == values[1].ToString().Trim() ;
            if (values.Length == 3)
                returnValues = values[0].ToString().Trim() == values[1]?.ToString().Trim() && (bool)values?[2] == true;
           

            return returnValues;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
