using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.ViewModels;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using System.Windows.Controls;
using System.Windows.Media;

namespace FF.Cockpit.UI.Views.UserControls.TrendsGraphExamined
{
    /// <summary>
    /// Interaction logic for ExaminationTrends_1Year.xaml
    /// </summary>
    public partial class SessionDurationTrends_1Year : UserControl
    {
        public SessionDurationTrends_1Year()
        {
            InitializeComponent();
            DataContext = this;
        }

        
    }
}

