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
    public class SkinTileBackgroundConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object returnValues = null;
            if (value != null)
            {
                switch (value)
                {
                    case "I":
                        returnValues = "#FFFFFF";
                        break;
                    case "II":
                        returnValues = "#5F5F61";
                        break;
                    case "III":
                        returnValues = "#BF967E";
                        break;
                    case "IV":
                        returnValues = "#8E7358";
                        break;
                    case "V":
                        returnValues = "#554432";
                        break;
                }
            }
            return returnValues;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
