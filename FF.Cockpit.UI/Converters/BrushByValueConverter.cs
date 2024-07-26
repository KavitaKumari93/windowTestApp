using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using FF.Cockpit.Common;

namespace FF.Cockpit.UI.Converters
{
    public class BrushByValueConverter : IMultiValueConverter
    {


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = Brushes.Transparent;

            int val;
            bool isValueParse = Int32.TryParse(values[0].ToString(), out val);
            
            if (isValueParse && values[1]!=null)
            {
                DateTime nextServiceDate = DateTime.ParseExact(values[1].ToString(), ConstantHelper.DateFormat, CultureInfo.InvariantCulture);

                //if (nextServiceDate < DateTime.Now.Date)
                //{
                //    brush = (Brush)new BrushConverter().ConvertFromString("#FF8F73");//red
                //}
                //else
                //{
                    if (val < 25)
                        brush = (Brush)new BrushConverter().ConvertFromString("#57D9A3");//green
                    else if (val >= 25 && val <= 50)
                        brush = (Brush)new BrushConverter().ConvertFromString("#FFFF00");//yellow
                    else if (val >= 51 && val <= 75)
                        brush = (Brush)new BrushConverter().ConvertFromString("#FF8000");//orange
                    else if (val > 75)
                        brush = (Brush)new BrushConverter().ConvertFromString("#DB0B12");//red
                    
                //}
            }

            return brush;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
