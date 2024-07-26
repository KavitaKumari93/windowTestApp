using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FF.Cockpit.Theme.Custom_Resources
{
    public static class ImageHelper
    {

        private static void SourceResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Image;
            if (element != null)
            {

                element.SetResourceReference(Image.SourceProperty, e.NewValue);
            }
        }

        public static readonly DependencyProperty SourceResourceKeyProperty = DependencyProperty.RegisterAttached("SourceResourceKey",
            typeof(object),
            typeof(ImageHelper),
            new PropertyMetadata(String.Empty, SourceResourceKeyChanged));

        public static void SetSourceResourceKey(Image element, object value)
        {

            element.SetValue(SourceResourceKeyProperty, value);
        }

        public static object GetSourceResourceKey(Image element)
        {

            return element.GetValue(SourceResourceKeyProperty);
        }
    }
}
