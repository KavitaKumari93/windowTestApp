using FF.Cockpit.Common;
using System.Windows.Controls;

namespace FF.Cockpit.UI.Views.UserControls
{
    /// <summary>
    /// Interaction logic for PerformanceUC.xaml
    /// </summary>
    public partial class PerformanceUC : UserControl
    {
        public PerformanceUC()
        {
            InitializeComponent();
        }

        private void PerformanceUC_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppStarter.IsPerformanceCalendarPopUpOpen != null && (AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen) && !AppStarter.IsPerformanceCalendarPopUpOpen.IsMouseCaptureWithin)
            {
                AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen = false;
            }

        }


        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }
}
