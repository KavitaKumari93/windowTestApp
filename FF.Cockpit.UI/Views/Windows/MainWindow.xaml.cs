using FF.Cockpit.Common;
using FF.Cockpit.Theme.Controls;
using System.Windows;

namespace FF.Cockpit.UI.Views.Windows
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        //private static readonly IUniverseMetadataProvider MetadataProvider = UniverseMetadataProvider.CreateUniverseMetadataProvider();
        public MainWindow()
        {
            InitializeComponent();
            AppStarter.MainWindow = this;
            leftNavigationTitle.IsOpen = true;
            flyoutTitle.Visibility = leftNavigationTitle.IsOpen ? Visibility.Visible : Visibility.Collapsed;

        }

        private void leftNavigation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            leftNavigationIcons.IsOpen = !leftNavigationIcons.IsOpen;
            flyoutIcon.Visibility = leftNavigationIcons.IsOpen ? Visibility.Visible : Visibility.Collapsed;

            leftNavigationTitle.IsOpen = !leftNavigationTitle.IsOpen;
            flyoutTitle.Visibility = leftNavigationTitle.IsOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MetroWindow_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            double windowHeight = SystemParameters.WorkArea.Height;
            double windowWidth = SystemParameters.WorkArea.Width;
            leftNavigationIcons.Width = windowWidth / 100 * 3.7;
            leftNavigationTitle.Width = windowWidth / 100 * 15;
        }

        /// <summary>
        /// Handles the Click event of the MinimizeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
                /// Handles the Click event of the MaximizeButton control.
                /// </summary>
                /// <param name="sender">The source of the event.</param>
                /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        /// <summary>
                /// Handles the Click event of the RestoreButton control.
                /// </summary>
                /// <param name="sender">The source of the event.</param>
                /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles the Click event of the Header drag control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Header_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MetroWindow_Deactivated(object sender, System.EventArgs e)
        {
            //close calender popup
            if (AppStarter.IsPerformanceCalendarPopUpOpen != null)
            {
                if (AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen)
                {
                    AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen = false;
                    AppStarter.IsPerformanceCalendarPopUpOpen = null;
                }
            }
            if (AppStarter.IsSchedulerCalendarPopUpOpen != null)
            {
                if (AppStarter.IsSchedulerCalendarPopUpOpen.IsOpen)
                    AppStarter.IsSchedulerCalendarPopUpOpen.IsOpen = false;
            }
        }

        private void lstTitlePanel_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AppStarter.IsNavigateToPatientDashboard = false;
        }

        private void MetroWindow_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (AppStarter.IsPerformanceCalendarPopUpOpen != null && (AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen) && !AppStarter.IsPerformanceCalendarPopUpOpen.IsMouseCaptureWithin)
            {
                AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen = false;
            }

        }
    }
}

