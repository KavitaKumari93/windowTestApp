using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace FF.Cockpit.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AppointmentEditor.xaml
    /// </summary>
    public partial class AppointmentEditor : Window
    {

        private readonly SchedulerViewModel _schedulerViewModel = null;
        private Dictionary<double, string> _startTimeList { get; set; }
        private Dictionary<double, string> _endTimeList { get; set; }
        private ObservableCollection<AppointmentTypeModel> _appointmentTypeList { get; set; }


        public AppointmentEditor(SchedulerViewModel schedulerViewModel)
        {
            _schedulerViewModel = schedulerViewModel;

            InitializeComponent();
            LoadMasterData();

            this.DataContext = schedulerViewModel;
            AppStarter.IsSchedulerCalendarPopUpOpen = this.startDatecalendarPopup;
        }

        #region methods

        private void LoadMasterData()
        {
            try
            {
                //Get Patient List
                //var patientData = await new AppointmentModel().GetPatientsInformation();
                //if (patientData != null && patientData.Count() > 0)
                //{
                //    //PatientsList = patientData.ToObservableCollection();
                //    //FilteredPatientList = PatientsList;
                //}

                //Get appointmentType List
                var appointmentTypeData = new AppointmentTypeModel().GetAppointmentTypesList_sync();
                if (appointmentTypeData != null && appointmentTypeData.Count() > 0)
                    _appointmentTypeList = appointmentTypeData.ToObservableCollection();
                this.appointmentTypeCMB.ItemsSource = _appointmentTypeList;

                //Get Rooms List
                var roomData = new RoomModel().GetRoomsList_sync();
                if (roomData != null && roomData.Count() > 0)
                    this.roomCMB.ItemsSource = roomData.ToObservableCollection();

                //Get Users List
                var usersData = new UsersInformationModel().GetUsersList_sync();
                var rolesData = new RoleModel().GetRolesList_sync();
                if (usersData != null && usersData.Count() > 0 && rolesData != null && rolesData.Count() > 0)
                {
                    var result = rolesData.FirstOrDefault(t => t.RoleName.ToLower().Contains("doctor"));
                    if(result!=null)
                    this.doctorCMB.ItemsSource = usersData.Where(x => x.UserRoleId == result.RoleId).ToObservableCollection();

                }


                //Get duration List
                this.durationCMB.ItemsSource = AppointmentRefData.durationData;

                //get RecurrenceType
                var recurrenceTypes = AppointmentRefData.recurrenceTypeData;
                this.recurrenceCMB.ItemsSource = recurrenceTypes;


                //select default
                if (_schedulerViewModel != null && _schedulerViewModel.IsNewAppointment)
                {
                    var selectedResource = _schedulerViewModel.SelectedView == CustomSchedulerViewType.Day ? _schedulerViewModel.SelectedResource_Day : _schedulerViewModel.SelectedResource;

                    _schedulerViewModel.SelectedDuration = AppointmentRefData.durationData.FirstOrDefault().Key;

                    _schedulerViewModel.SelectedAppointmentTypeId = appointmentTypeData.FirstOrDefault().AppointmentTypeId;
                    _schedulerViewModel.SelectedRoomId = (selectedResource.IsRoomResource && selectedResource.ResourceId > 0) ? selectedResource.ResourceId : roomData.FirstOrDefault().RoomId;
                    _schedulerViewModel.SelectedUserId = (!selectedResource.IsRoomResource && selectedResource.ResourceId > 0) ? selectedResource.ResourceId : usersData.FirstOrDefault().UserId;

                    _schedulerViewModel.SelectedAppointmentStartTime = _startTimeList.OrderBy(x => Math.Abs(x.Key - DateTime.Now.TimeOfDay.TotalMinutes)).FirstOrDefault().Key;
                    _schedulerViewModel.SelectedAppointmentEndTime = _endTimeList.OrderBy(x => Math.Abs(x.Key - _schedulerViewModel.SelectedAppointmentStartTime)).FirstOrDefault().Key;


                    _schedulerViewModel.SelectedRecurrenceMonth = recurrenceTypes.FirstOrDefault().Key;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GetTimeZone()
        {
            // this.TimeZoneMenu.ItemsSource = new List<string>()
            // {
            //    "Samoa Standard Time",
            //    "Dateline Standard Time",
            //    "UTC-11",
            //    "Hawaiian Standard Time",
            //    "Alaskan Standard Time",
            //    "Pacific Standard Time",
            //    "Pacific Standard Time (Mexico)",
            //    "Mountain Standard Time",
            //    "Mountain Standard Time (Mexico)",
            //    "US Mountain Standard Time",
            //    "Canada Central Standard Time",
            //    "Central America Standard Time",
            //    "Central Standard Time",
            //    "Eastern Standard Time",
            //    "SA Pacific Standard Time",
            //    "US Eastern Standard Time",
            //    "Venezuela Standard Time",
            //    "Atlantic Standard Time",
            //    "Central Brazilian Standard Time",
            //    "Pacific SA Standard Time",
            //    "Paraguay Standard Time",
            //    "SA Western Standard Time",
            //    "Newfoundland Standard Time",
            //    "Argentina Standard Time",
            //    "Bahia Standard Time",
            //    "Greenland Standard Time",
            //    "E. South America Standard Time",
            //    "Montevideo Standard Time",
            //    "SA Eastern Standard Time",
            //    "UTC-02",
            //    "(UTC - 01:00) Azores Standard Time",
            //    "(UTC - 01:00) Cape Verde Standard Time",
            //    "(UTC) GMT Standard Time",
            //    "(UTC) Greenwich Standard Time",
            //    "(UTC) Morocco Standard Time",
            //    "(UTC) UTC",
            //    "Magadan Standard Time",
            //    "New Zealand Standard Time",
            //    "Russia Time Zone 11",
            //    "UTC+12",
            //    "Line Islands Standard Time",
            //    "Tonga Standard Time",
            //};
        }
        private void OnAddRememainderClicked(object sender, RoutedEventArgs e)
        {

            //if (this.ReminderList != null)
            //{
            //    var reminders = this.ReminderList.ItemsSource as IList;
            //    this.ReminderList.ItemContainerGenerator.StatusChanged += this.OnListViewItemGeneratorStatusChanged;
            //    if (reminders == null)
            //        reminders = new ObservableCollection<SchedulerReminder>();
            //    else if (reminders.Count == 5)
            //    {
            //        // Only maximum of 5 reminders allowed in editor window.
            //        return;
            //    }
            //    var newRemainder = new SchedulerReminder();
            //    reminders.Add(newRemainder);
            //    this.ReminderList.ItemsSource = reminders;
            //}
        }
        private void OnRemoveReminderClicked(object sender, RoutedEventArgs e)
        {
            //var button = sender as Button;
            //var reminderCollection = this.ReminderList.ItemsSource as IList;
            //reminderCollection.Remove(button.DataContext as SchedulerReminder);
        }
        private void OnTimeZoneChecked(object sender, RoutedEventArgs e)
        {
            //if (this.timeZone.IsChecked == true)
            //    this.TimeZoneMenuPanel.Visibility = Visibility.Visible;
            //else
            //    this.TimeZoneMenuPanel.Visibility = Visibility.Visible;
        }
        private void startTimeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.startTimeList?.SelectedValue != null)
            {
                if (this.durationCMB.SelectedValue != null)
                {
                    if ((double)this.startTimeList.SelectedValue + (double)this.durationCMB.SelectedValue > 1440)
                        ResetEndDateAndTime();

                    else
                    {
                        this.endTimeList.SelectedValue = (double)this.startTimeList.SelectedValue + (double)this.durationCMB.SelectedValue;
                        var format = "MM/dd/yyyy"; var dateString = this.startDateTxtblock.Text;
                        var formatInfo = new DateTimeFormatInfo()
                        {
                            ShortDatePattern = format
                        };
                        var formatDate = Convert.ToDateTime(dateString, formatInfo);
                        var finalDate = formatDate.AddMinutes((double)this.durationCMB.SelectedValue);
                        var formatedDate = finalDate.ToString("MM-dd-yyyy");
                        this.endDateTxtbox.Text = formatedDate.Replace('-', '/');
                    }
                }

            }
        }
        private void endTimeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //if (this.startTimeList?.SelectedValue != null && this.endTimeList?.SelectedValue != null)
            //{
            //    double timeDiff = CalculateDuration(durationData);
            //    if (timeDiff >= 0)
            //        this.durationCMB.SelectedValue = CalculateDuration(durationData);

            //}

        }

        private void durationCMB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.durationCMB.SelectedValue == null)
                this.durationCMB.SelectedValue = AppointmentRefData.durationData.FirstOrDefault().Key;
            ResetEndDateAndTime();
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
        }
        private void ResetEndDateAndTime()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.startDateTxtblock.Text))
                {
                    //get time List
                    _startTimeList = new Dictionary<double, string>();
                    _startTimeList = AppointmentRefData.GetStartTimeList((double)this.durationCMB.SelectedValue);
                    this.startTimeList.ItemsSource = _startTimeList;

                    _endTimeList = new Dictionary<double, string>();
                    _endTimeList = AppointmentRefData.GetEndTimeList();
                    this.endTimeList.ItemsSource = _endTimeList;



                    var format = "MM/dd/yyyy";
                    var dateString = this.startDateTxtblock.Text;
                    var formatInfo = new DateTimeFormatInfo()
                    {
                        ShortDatePattern = format
                    };

                    var formatDate = Convert.ToDateTime(dateString, formatInfo);

                    if (this.startTimeList?.SelectedValue == null)
                        this.startTimeList.SelectedValue = _startTimeList.LastOrDefault().Key;

                    var startDate = formatDate.AddMinutes((double)this.startTimeList?.SelectedValue);

                    var duration = startDate.AddMinutes((double)this.durationCMB.SelectedValue);
                    this.endTimeList.SelectedValue = (double)duration.TimeOfDay.TotalMinutes;
                    var finalDate = duration.Date.AddMinutes((double)this.durationCMB.SelectedValue);
                    var formatedDate = finalDate.ToString("MM-dd-yyyy");
                    this.endDateTxtbox.Text = formatedDate.Replace('-', '/');

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}


