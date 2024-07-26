using FF.Cockpit.Common;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Linq;

namespace FF.Cockpit.UI.Converters
{
    /// <summary>
    /// Purpose : Added for the Anonymization of string , 
    /// ref. eg. Suppose we have name like "Jacob Gottlieb" this will return "J. G."
    /// and  in case we have only single name:"Jacob " => "J."
    /// </summary>
    public class AnonymizeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = (string)value;
            if (!string.IsNullOrEmpty(stringValue))
            {
                List<string> splitNames = stringValue.Split(' ').ToList();
                if(splitNames!=null && splitNames.Count()>1)
                {
                    string anonymizeName = string.Empty;
                    foreach (var name in splitNames)
                    {
                        anonymizeName += !string.IsNullOrEmpty(name)? name.Substring(0, 1) + "."+" ":string.Empty;
                    }
                    return AppStarter.IsPatientNameAnontmized ? anonymizeName : stringValue;
                }
                else
                    return AppStarter.IsPatientNameAnontmized ? splitNames[0].Substring(0, 1) + "." : stringValue;
            }
            else
                return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
