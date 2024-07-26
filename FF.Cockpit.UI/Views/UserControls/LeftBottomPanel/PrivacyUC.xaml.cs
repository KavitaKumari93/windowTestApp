using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.LeftBottomPanel
{
    /// <summary>
    /// Interaction logic for PrivacyUC.xaml
    /// </summary>
    public partial class PrivacyUC : UserControl
    {
        public PrivacyUC()
        {
            InitializeComponent();
            this.DataContext = new LeftBottomMenuViewModel();
        }
    }
}
