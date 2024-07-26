using FF.Cockpit.UI.ViewModels;
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
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.UserControls.CloudSync
{
    /// <summary>
    /// Interaction logic for UpdateDownOrUploadDate.xaml
    /// </summary>
    public partial class UpdateDownOrUploadDate : Window
    {
        public UpdateDownOrUploadDate(Object vm)
        {
            InitializeComponent();
            this.DataContext =vm;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
