using FF.Cockpit.UI.ViewModels;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls.LeftBottomPanel
{
    /// <summary>
    /// Interaction logic for ContactUC.xaml
    /// </summary>
    public partial class ContactUC : UserControl
    {
        public ContactUC()
        {
            InitializeComponent();
            this.DataContext = new LeftBottomMenuViewModel();
        }
    }
}
