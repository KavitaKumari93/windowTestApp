using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FF.Cockpit.Theme.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Liefert oder setzt ein Flag, welches angibt, ob anstelle von <see cref="Visibility.Collapsed"/> der Wert <see cref="Visibility.Hidden"/>
        /// verwendet werden soll.
        /// </summary>
        public bool UseVisibilityHidden { get; set; } = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility result = Visibility.Visible;

            if (parameter == null)
            {
                result = (value == null) ? (this.UseVisibilityHidden ? Visibility.Hidden : Visibility.Collapsed) : Visibility.Visible;
            }
            else
            {
                result = (value == null) ? Visibility.Visible : (this.UseVisibilityHidden ? Visibility.Hidden : Visibility.Collapsed);
            }

            return result;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
