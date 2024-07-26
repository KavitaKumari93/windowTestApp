using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.Configuration
{
    /// <summary>
    /// Interaction logic for AppointmentTypeConfigurationView.xaml
    /// </summary>
    public partial class AppointmentTypeConfigurationView : UserControl
    {
        public AppointmentTypeConfigurationView(ConfigurationViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;

        }
    }
}
