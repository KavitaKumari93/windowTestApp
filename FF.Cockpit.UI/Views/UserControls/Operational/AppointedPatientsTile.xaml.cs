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

namespace FF.Cockpit.UI.Views.UserControls.Operational
{
    /// <summary>
    /// Interaction logic for AppointedPatientsTile.xaml
    /// </summary>
    public partial class AppointedPatientsTile : UserControl
    {
        public AppointedPatientsTile()
        {
            InitializeComponent();
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 30) / 100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 25) / 100;
            dataGrid.Columns[2].Width = (userCtrlWindth * 10) / 100;
            dataGrid.Columns[3].Width = (userCtrlWindth * 17) / 100;
            dataGrid.Columns[4].Width = (userCtrlWindth * 18) / 100;
           

        }
    }
}
