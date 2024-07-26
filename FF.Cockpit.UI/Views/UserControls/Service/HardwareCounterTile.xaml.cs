using System.Windows;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.Service
{
    /// <summary>
    /// Interaction logic for HardwareCounterTile.xaml
    /// </summary>
    public partial class HardwareCounterTile : UserControl
    {
        public HardwareCounterTile()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 23) / 100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 9) / 100;
            dataGrid.Columns[2].Width = (userCtrlWindth * 11) / 100;
            dataGrid.Columns[3].Width = (userCtrlWindth * 12) / 100;
            dataGrid.Columns[4].Width = (userCtrlWindth * 12) / 100;
            dataGrid.Columns[5].Width = (userCtrlWindth * 9) / 100;
            dataGrid.Columns[6].Width = (userCtrlWindth * 25) / 100;
        }
    }
}
