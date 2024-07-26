using FF.Cockpit.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls
{
    /// <summary>
    /// Interaction logic for RoomConfigurationView.xaml
    /// </summary>
    public partial class RoomConfigurationView : UserControl
    {
        public RoomConfigurationView(ConfigurationViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 30) / 100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 66) / 100;
            //dataGrid.Columns[2].Width = (userCtrlWindth * 20) / 100;

        }
    }
}
