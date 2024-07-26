using FF.Cockpit.Common;
using FF.Cockpit.UI.Helpers;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Linq;
using System.Windows.Controls;

namespace FF.Cockpit.UI.CustomControl
{
    /// <summary>
    /// Interaction logic for OperationalSchedulerTile.xaml
    /// </summary>
    public partial class SchedulerControl : UserControl
    {
        public SchedulerControl()
        {
            InitializeComponent();
            this.Schedule.ViewType = (SchedulerViewType)AppStarter.common_SelectedView;
            this.Schedule.DisplayDate = AppStarter.common_SelectedDate;

            ////Get data from configuration

            var startHourObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.StartHour.ToString())?.Value;
            var endHourObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.EndHour.ToString())?.Value;
            var dayOfWeekObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.FirstDayOfWeek.ToString())?.Value;

            this.Schedule.DaysViewSettings.TimeRulerFormat = "HH:mm";
            this.Schedule.DaysViewSettings.StartHour = startHourObj != null ? Convert.ToDouble(startHourObj) / 60 : 6;
            this.Schedule.DaysViewSettings.EndHour = endHourObj != null ? Convert.ToDouble(endHourObj) / 60 : 22;

            DayOfWeek dayOfWeek;
            this.Schedule.FirstDayOfWeek = Enum.TryParse(dayOfWeekObj?.ToString(), out dayOfWeek) ? dayOfWeek : DayOfWeek.Monday;
        }

        private void Schedule_SelectionChanging(object sender, SelectionChangingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
