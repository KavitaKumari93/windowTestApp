using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FF.Cockpit.UI.Views.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for ExcisedMarkerListTile.xaml
    /// </summary>
    public partial class ExcisedMarkerListTile : Window
    {
        private readonly DashboardViewModel _dashboardViewModel = null;
        public ExcisedMarkerListTile(DashboardViewModel dashboardViewModel)
        {
            _dashboardViewModel= dashboardViewModel;
            InitializeComponent();
            this.DataContext = dashboardViewModel;
        }
    }
}
