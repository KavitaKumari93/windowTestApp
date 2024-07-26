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
    public class IntergralConverter:IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object returnValues = null;
            returnValues = IntergralConvert(value?.ToString().Trim());
            return returnValues;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        public string IntergralConvert(string value)
        {
            string returnValues = null;
            if (value != null)
            {
                switch (value)
                {
                    case "I":    returnValues = "1";  break;
                    case "II":   returnValues = "2";  break;
                    case "III":  returnValues = "3";  break;
                    case "IV":   returnValues = "4";  break;
                    case "V":    returnValues = "5";  break;
                }
            }
            return returnValues;
        }
    }
}
