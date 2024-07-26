using FF.Cockpit.UI.ViewModels;
using FF.Cockpit.UI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.Windows
{
    /// <summary>
    /// Interaction logic for SystemLoginWindow.xaml
    /// </summary>
    public partial class SystemLoginWindow : Window
    {

        SystemLoginViewModel viewModel;
        #region Constructorss
        public SystemLoginWindow()
        {
            this.InitializeComponent();
            viewModel = new SystemLoginViewModel();
            this.DataContext = viewModel;
        }

        #endregion

    }
}
