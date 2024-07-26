using System.Windows;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.Operational
{
    /// <summary>
    /// Interaction logic for AppointedDoctorsTile.xaml
    /// </summary>
    public partial class AppointedDoctorsTile : UserControl
    {
        public AppointedDoctorsTile()
        {
            InitializeComponent();
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 23) / 100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 23) / 100;
            dataGrid.Columns[2].Width = (userCtrlWindth * 31) / 100;
            dataGrid.Columns[3].Width = (userCtrlWindth * 23) / 100;
           // dataGrid.Columns[4].Width = (userCtrlWindth * 18) / 100;
        }
    }
}
