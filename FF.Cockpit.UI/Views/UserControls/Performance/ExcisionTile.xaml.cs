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

namespace FF.Cockpit.UI.Views.UserControls.Performance
{
    /// <summary>
    /// Interaction logic for ExcisionTile.xaml
    /// </summary>
    public partial class ExcisionTile : UserControl
    {
        public ExcisionTile()
        {
            InitializeComponent();
            
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = rowHeight.ActualHeight;
            viewCount.Height = height;
        }
    }
}
