using FF.Cockpit.Common;
using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace FF.Cockpit.UI.Views.UserControls.Configuration
{
    /// <summary>
    /// Interaction logic for MiscellaneousConfigurationView.xaml
    /// </summary>
    public partial class MiscellaneousConfigurationView : UserControl
    {
        public MiscellaneousConfigurationView(ConfigurationViewModel vm)
        {
            InitializeComponent();
            errorMessage_NoOfDays.Text = "Please enter the no of days for sync cloud.";
            this.starthourList.ItemsSource = GetStartHourList();
            this.endhourList.ItemsSource = GetStartHourList();
            this.dayOfWeekList.ItemsSource = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
            this.DataContext = vm;
            this.chkAnomizedPatientName.IsChecked = AppStarter.IsPatientNameAnontmized;
            this.chkServiceTabVisibility.IsChecked = AppStarter.IsServiceTabVisible;
        }

        private static Dictionary<double, string> GetStartHourList()
        {
            Dictionary<double, string> dic = new Dictionary<double, string>();
            double fromTime = 60, toTime = 1440;

            for (double i = fromTime; i <= toTime; i++)
            {
                var time = TimeSpan.FromMinutes(i);
                dic.Add(i, string.Format("{0:00}:{1:00}", (int)time.TotalHours, time.Minutes));
                i += 59;
            }

            //dic.Add(00, "00:00");
            //dic.Add(60, "01:00");
            //dic.Add(120, "02:00");
            //dic.Add(180, "03:00");
            //dic.Add(240, "04:00");
            //dic.Add(300, "05:00");
            //dic.Add(360, "06:00");
            //dic.Add(420, "07:00");
            //dic.Add(480, "08:00");
            //dic.Add(540, "09:00");
            //dic.Add(600, "10:00");
            //dic.Add(660, "11:00");
            //dic.Add(720, "12:00");
            //dic.Add(780, "13:00");
            //dic.Add(840, "14:00");
            //dic.Add(900, "15:00");
            //dic.Add(960, "16:00");
            //dic.Add(1020, "17:00");
            //dic.Add(1080, "18:00");
            //dic.Add(1140, "19:00");
            //dic.Add(1200, "20:00");
            //dic.Add(1260, "21:00");
            //dic.Add(1320, "22:00");
            ////dic.Add(1380, "23:00");

            return dic;
        }

        private void starthourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double startHour = starthourList.SelectedValue != null ? (double)starthourList.SelectedValue : 0;
            double endHour = endhourList.SelectedValue != null ? (double)endhourList.SelectedValue : 0;

            if (startHour < endHour)
            {
                errorMessage.Text = "";
            }
            else
            {
                errorMessage.Text = "Start hour should be less than end hour.";
                e.Handled = false;
            }
        }

        private void endhourList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double startHour = starthourList.SelectedValue != null ? (double)starthourList.SelectedValue : 0;
            double endHour = endhourList.SelectedValue != null ? (double)endhourList.SelectedValue : 0;

            if (endHour > startHour)
            {
                errorMessage.Text = "";
            }
            else
            {
                errorMessage.Text = "End hour should be greater than start hour.";
                e.Handled = false;
            }
        }

        private void txt_NoOfDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex validateNumberRegex = new Regex("^(\\d?[1-9]|[1-9]0)$");
            if (sender != null)
            {
                TextBox textBox = (TextBox)sender;
                errorMessage_NoOfDays.Text = validateNumberRegex.IsMatch(textBox.Text) ? "" : "Please enter day number from 1 to 99 only.";
            }
        }

        
    }
}
