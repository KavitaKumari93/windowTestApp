using FF.Cockpit.Common;
using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.LeftBottomPanel
{
    /// <summary>
    /// Interaction logic for AboutUC.xaml
    /// </summary>
    public partial class AboutUC : UserControl
    {
        public AboutUC()
        {
            InitializeComponent();
            versionNumber.Text = ConstantHelper.CockpitFullVersion;
        }
    }
}
