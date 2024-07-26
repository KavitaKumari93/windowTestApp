using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for MedicalHistoryTile.xaml
    /// </summary>
    public partial class MedicalHistoryTile : UserControl
    {
        public MedicalHistoryTile()
        {
            InitializeComponent();
            
        }
        private void imgSave_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            img.Focusable = true;
            img.Focus();
        }
        //private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    TextBox txt = sender as TextBox;
        //    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        //}
    }
}
