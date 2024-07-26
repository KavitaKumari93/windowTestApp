using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.Configuration
{
    /// <summary>
    /// Interaction logic for RolePermissionConfigurationView.xaml
    /// </summary>
    public partial class RolePermissionConfigurationView : UserControl
    {
        public RolePermissionConfigurationView(ConfigurationViewModel vm)
        {
           
            InitializeComponent();
            this.DataContext = vm;
            cmbo_Roles.SelectedIndex = 0;
        }
        private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            double userCtrlWindth = this.ActualWidth;
            dataGrid.Columns[0].Width = (userCtrlWindth * 40) / 100;
            dataGrid.Columns[1].Width = (userCtrlWindth * 30) / 100;
        }
    }
}
