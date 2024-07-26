using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.LeftBottomPanel
{
    /// <summary>
    /// Interaction logic for TermsofServiceUC.xaml
    /// </summary>
    public partial class TermsofServiceUC : UserControl
    {
        public TermsofServiceUC()
        {
            InitializeComponent();
            this.DataContext = new LeftBottomMenuViewModel();
        }
    }
}
