using System.Windows;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for ExaminationHistoryTile.xaml
    /// </summary>
    public partial class ExaminationHistoryTile : UserControl
    {
        public ExaminationHistoryTile()
        {
            InitializeComponent();
            
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 20)/100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 12)/100;
            dataGrid.Columns[2].Width = (userCtrlWindth * 10)/100;
            dataGrid.Columns[3].Width = (userCtrlWindth * 12)/100;
            dataGrid.Columns[4].Width = (userCtrlWindth * 10)/100;
            dataGrid.Columns[4].Width = (userCtrlWindth * 30)/100;

        }
    }
}
