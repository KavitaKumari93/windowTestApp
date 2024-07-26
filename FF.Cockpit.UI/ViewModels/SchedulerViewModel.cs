using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.UserControls;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class SchedulerViewModel : PropertyChangeHelper
    {

        #region Properties
        private string _previousSearchedString;
        public string PreviousSearchedString
        {
            get { return _previousSearchedString; }
            set { _previousSearchedString = value; OnPropertyChanged(); }
        }
        private bool _isDataFound;
        public bool IsDataFound
        {
            get { return _isDataFound; }
            set { _isDataFound = value; OnPropertyChanged(); }
        }

        public IEnumerable<SchedulerTypes> SchedulerTypesList
        {
            get { return Enum.GetValues(typeof(SchedulerTypes)).Cast<SchedulerTypes>(); }
        }

        public IEnumerable<CustomSchedulerViewType> CustomSchedulerViewTypeList
        {
            get { return Enum.GetValues(typeof(CustomSchedulerViewType)).Cast<CustomSchedulerViewType>(); }
        }

        private SchedulerViewType _schedulerView;
        public SchedulerViewType SchedulerView
        {
            get { return _schedulerView; }
            set { _schedulerView = value; OnPropertyChanged(); }
        }

        private bool IsOneTimeDateUpdate { get; set; }

        private DateTime _schedulerDate;
        public DateTime SchedulerDate
        {
            get { return _schedulerDate; }
            set
            {
                _schedulerDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime _calenderDisplayDate;
        public DateTime CalenderDisplayDate
        {
            get { return _calenderDisplayDate; }
            set
            {
                if (IsOneTimeDateUpdate)
                {
                    IsOneTimeDateUpdate = false;
                    _calenderDisplayDate = value;
                    SchedulerDate = CalenderDisplayDate;
                }
                OnPropertyChanged();
            }
        }
        private DateTime _selectedFromDate;
        public DateTime SelectedFromDate
        {
            get { return _selectedFromDate; }
            set { _selectedFromDate = value; OnPropertyChanged(); }
        }
     
        private DateTime _selectedToDate;
        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set { _selectedToDate = value; OnPropertyChanged(); }
        }
        
        //private bool _isPopUpOpen;
        //public bool IsPopUpOpen
        //{
        //    get { return _isPopUpOpen; }
        //    set { _isPopUpOpen = value; OnPropertyChanged(); }
        //}

        private ObservableCollection<ResourceModel> _resourceList_Day;
        public ObservableCollection<ResourceModel> ResourceList_Day
        {
            get { return _resourceList_Day; }
            set { _resourceList_Day = value; OnPropertyChanged(); }
        }
        private ResourceModel _selectedResource_Day;
        public ResourceModel SelectedResource_Day
        {
            get { return _selectedResource_Day; }
            set { _selectedResource_Day = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ResourceModel> _resourceList;
        public ObservableCollection<ResourceModel> ResourceList
        {
            get { return _resourceList; }
            set { _resourceList = value; OnPropertyChanged(); }
        }

        private ResourceGroupType _resourceGroup;
        public ResourceGroupType ResourceGroup
        {
            get { return _resourceGroup; }
            set { _resourceGroup = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AppointmentModel> _appointmentList;
        public ObservableCollection<AppointmentModel> AppointmentList
        {
            get { return _appointmentList; }
            set { _appointmentList = value; OnPropertyChanged(); }
        }




        //Data will be filter as per below selections
        private CustomSchedulerViewType _selectedView;
        public virtual CustomSchedulerViewType SelectedView
        {
            get { return _selectedView; }
            set
            {
                if (!AppStarter.IsNavigateToPatientDashboard)
                {
                    _selectedView = value;
                    AppStarter.common_SelectedView = value;
                }
                else
                {
                    _selectedView = AppStarter.common_SelectedView;
                }
                SchedulerView = (SchedulerViewType)SelectedView;

                SelectedDate = SelectedDate == DateTime.MinValue ? DateTime.Today : SelectedDate;
                OnPropertyChanged();

                //SetSchedulerFromOrToDate();
                //LoadData();
                //SchedulerFilterEvent.RaiseSchedulerFilterEvent();
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                IsOneTimeDateUpdate = true;
                if (!AppStarter.IsNavigateToPatientDashboard && SelectedDate != DateTime.MinValue)
                {
                    _selectedDate = value;
                    SchedulerDate = SelectedDate;
                    CalenderDisplayDate = SelectedDate;
                    AppStarter.common_SelectedDate = SelectedDate;
                    
                }
                else
                {
                    _selectedDate = AppStarter.common_SelectedDate;
                    SchedulerDate = AppStarter.common_SelectedDate;
                    CalenderDisplayDate = AppStarter.common_SelectedDate;
                }
                //IsPopUpOpen = false;
                SetSchedulerFromOrToDate(SelectedDate);
                LoadData();
                OnPropertyChanged();

                //Call data for operational view
                SchedulerFilterEvent.RaiseSchedulerFilterEvent();
            }
        }

        private ResourceModel _selectedResource;
        public ResourceModel SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                _selectedResource = value;

                foreach (var item in ResourceList)
                {
                    if (item.ResourceId == SelectedResource.ResourceId) item.Opacity = 1;
                    else item.Opacity = .5;
                }

                AppStarter.common_SelectedResource = value;

                ResourceGroup = ResourceGroupType.None;
                GetAppointments_WeekOrMonth();
                OnPropertyChanged();
            }
        }


        private SchedulerTypes _selectedSchedulerType = SchedulerTypes.RoomView;
        public SchedulerTypes SelectedSchedulerType
        {
            get { return _selectedSchedulerType; }
            set
            {
                mainViewModel.SetLoaderCountZero();

                _selectedSchedulerType = value;
                if (SelectedView == CustomSchedulerViewType.Day)
                {
                    ResourceGroup = ResourceGroupType.Resource;
                    GetResourcesWithAppointments_Day();
                }
                else
                {
                    ResourceGroup = ResourceGroupType.None;
                    GetResources_WeekOrMonth();
                    GetAppointments_WeekOrMonth();
                }
                OnPropertyChanged();
            }
        }

        private AppointedPatient _selectedAppointedPatient;
        public AppointedPatient SelectedAppointedPatient
        {
            get { return _selectedAppointedPatient; }
            set
            {
                _selectedAppointedPatient = value;
                if (SelectedView == CustomSchedulerViewType.Day)
                {
                    ResourceGroup = ResourceGroupType.Resource;
                    GetResourcesWithAppointments_Day();
                }
                else
                {
                    ResourceGroup = ResourceGroupType.None;
                    SelectedResource = ResourceList.FirstOrDefault(x => x.ResourceId == (SelectedResource.IsRoomResource ? value?.RoomId : 0));
                }
                OnPropertyChanged();
            }
        }

        private AppointedDoctor _selectedAppointmentedDoctor;
        public AppointedDoctor SelectedAppointmentedDoctor
        {
            get { return _selectedAppointmentedDoctor; }
            set { _selectedAppointmentedDoctor = value; OnPropertyChanged(); }
        }

        #endregion

        #region Constructor

        public MainViewModel mainViewModel { get; set; }
        public SchedulerViewModel()
        {
            try
            {
                mainViewModel = (MainViewModel)AppStarter.mainVM;

                //************************ Appointment Calender *************************************
                ResourceList_Day = new ObservableCollection<ResourceModel>();
                ResourceList = new ObservableCollection<ResourceModel>();
                AppointmentList = new ObservableCollection<AppointmentModel>();

                DateNavigationForwardCommand = new RelayCommandHelper(x => DateForward());
                DateNavigationBackwordCommand = new RelayCommandHelper(x => DateBackward());
                AppointmentEditorOpenCommand = new RelayCommandHelper<AppointmentEditorOpeningEventArgs>(OpenAppointmentEditor);
                AppointmentDroppingCommand = new RelayCommandHelper<AppointmentDroppingEventArgs>(DropAppointment);
                AppointmentResizingCommand = new RelayCommandHelper<AppointmentResizingEventArgs>(ResizeAppointment);


                //************************ Appointment Popup *************************************
                Appointment = new AppointmentModel();
                OpenCalendarCommand = new RelayCommandHelper(x => OpenCalendar(x));
                SaveCommand = new RelayCommandHelper(x => OnSave());
                CancelCommand = new RelayCommandHelper(x => OnCancel());
                DeleteCommand = new RelayCommandHelper(x => OnDelete());
                ClosingCommand = new RelayCommandHelper<CancelEventArgs>(Closing);

                //************************ AutoComplete over  Appointment Popup *************************************
                AutoCompletorListSelectionChangedCommand = new RelayCommandHelper(x => autoCompletorList_SelectionChanged(x));
                DocumentType_LostFocusCommand = new RelayCommandHelper(x => DocumentType_LostFocus(x));
                DocumentType_KeyUpCommand = new RelayCommandHelper(x => DocumentType_KeyUp(x));

                AppStarter.common_SchedulerType = SchedulerTypes.RoomView;

                IsDataFound = (new RoomModel().GetRoomsList_sync().Count() > 0 && new UsersInformationModel().GetUsersList_sync().Count() > 0 && new AppointmentTypeModel().GetAppointmentTypesList_sync().Count() > 0);
                SelectedView = AppStarter.common_SelectedView;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        #endregion

        #region Scheduler Filter Actions
        private void DateForward()
        {
            var date = SelectedDate;
            switch (SchedulerView)
            {
                case SchedulerViewType.Day:
                    SelectedDate = date.AddDays(1);
                    break;
                case SchedulerViewType.Week:
                    SelectedDate = date.FirstDayOfNextWeek();
                    break;
                case SchedulerViewType.Month:
                    SelectedDate = date.AddMonths(1);
                    break;
            }
        }
        private void DateBackward()
        {
            var date = SelectedDate;
            switch (SchedulerView)
            {
                case SchedulerViewType.Day:
                    SelectedDate = date.AddDays(-1);
                    break;
                case SchedulerViewType.Week:
                    SelectedDate = date.FirstDayOfPreviousWeek();
                    break;
                case SchedulerViewType.Month:
                    SelectedDate = date.AddMonths(-1);
                    break;
            }
        }
        private void SetSchedulerFromOrToDate(DateTime selectedDate)
        {
            switch (SelectedView)
            {
                case CustomSchedulerViewType.Day:
                    SelectedFromDate = selectedDate;
                    SelectedToDate = selectedDate;
                    break;

                case CustomSchedulerViewType.Week:
                    SelectedFromDate = selectedDate.StartOfWeek(DayOfWeek.Monday);
                    SelectedToDate = _selectedFromDate.AddDays(6);
                    break;

                case CustomSchedulerViewType.Month:
                    SelectedFromDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    SelectedToDate = _selectedFromDate.AddMonths(1).AddSeconds(-1);
                    break;
            }
        }

        #endregion

        #region Load Resources and respective appointments
        public void LoadData()
        {
            if (SelectedView == CustomSchedulerViewType.Day)
            {
                ResourceGroup = ResourceGroupType.Resource;
                GetResourcesWithAppointments_Day();
            }
            else
            {
                ResourceGroup = ResourceGroupType.None;
                GetResources_WeekOrMonth();
                GetAppointments_WeekOrMonth();
            }
        }
        private void GetResources_WeekOrMonth()
        {
            ResourceList = new ObservableCollection<ResourceModel>();
            var resourceData = new ResourceModel().GetResourcesList_sync(SelectedSchedulerType);
            if (resourceData != null && resourceData.Count() > 0)
            {
                ResourceList = resourceData.ToObservableCollection<ResourceModel>();

                //if (mainViewModel.SelectedMenu.Id == 2)
                ResourceList.Insert(0, new ResourceModel() { ResourceId = 0, ResourceName = localisation.Scheduler_ConsolidateView_resx, IsRoomResource = SelectedSchedulerType == SchedulerTypes.RoomView });

                if (SelectedSchedulerType != AppStarter.common_SchedulerType)
                    AppStarter.common_SelectedResource = null;

                SelectedResource = AppStarter.common_SelectedResource != null
                    ? (ResourceModel)AppStarter.common_SelectedResource
                    : ResourceList.FirstOrDefault();

            }
        }
        private void GetAppointments_WeekOrMonth()
        {
            if (SelectedFromDate != DateTime.MinValue)
            {
                mainViewModel.SetLoaderCountZero();

                AppointmentList = new ObservableCollection<AppointmentModel>();

                int? patientId = null;
                int? roomId = null;
                int? doctorId = null;

                var res = SelectedResource;
                var patient = SelectedAppointedPatient;

                if (res?.ResourceId > 0)
                    switch (SelectedSchedulerType)
                    {
                        case SchedulerTypes.RoomView:
                            patientId = patient?.PatientId;
                            roomId = res?.ResourceId;
                            doctorId = null;
                            break;
                        case SchedulerTypes.DoctorView:
                            patientId = patient?.PatientId;
                            roomId = null;
                            doctorId = res?.ResourceId;
                            break;
                    }

                var task = Task.Factory.StartNew(() => { mainViewModel.LoaderShow(); });
                task.ContinueWith(async (priorTask) =>
                {
                    mainViewModel.TotalCallMethodCount++;

                    var appointmentData = await Task.Run(() => new AppointmentModel().GetAppointmentList(SelectedFromDate, SelectedToDate, patientId, roomId, doctorId));

                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (appointmentData != null && appointmentData.Count() > 0)
                        {
                            appointmentData.ToList().ForEach(x =>
                            {
                                x.AppointmentBackground = (SolidColorBrush)new BrushConverter().ConvertFrom(x.BackgroundColor);
                                x.StartTime = x.StartTime.ToLocalTime();
                                x.EndTime = x.EndTime.ToLocalTime();
                                x.StartTimeStr = x.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                                x.EndTimeStr = x.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                                x.Duration = x.EndTime.Subtract(x.StartTime).TotalMinutes;
                            });
                            AppointmentList = appointmentData.ToObservableCollection();
                        }
                    }));
                    mainViewModel.CalledMethodCount++;
                });
                task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
            }
        }
        private void GetResourcesWithAppointments_Day()
        {
            mainViewModel.TotalCallMethodCount = 0;
            mainViewModel.CalledMethodCount = 0;

            ResourceList_Day = new ObservableCollection<ResourceModel>();
            AppointmentList = new ObservableCollection<AppointmentModel>();

            int? patientId = SelectedAppointedPatient?.PatientId;

            var task = Task.Factory.StartNew(() => { mainViewModel.LoaderShow(); });
            task.ContinueWith(async (priorTask) =>
            {
                mainViewModel.TotalCallMethodCount++;

                var resourceData = await Task.Run(() => new ResourceModel().GetResourcesList(SelectedSchedulerType));
                var appointmentData = await Task.Run(() => new AppointmentModel().GetAppointmentList(SelectedFromDate, SelectedToDate, patientId, null, null));


                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    if (resourceData != null && resourceData.Count() > 0)
                    {
                        ResourceList_Day = resourceData.ToObservableCollection<ResourceModel>();
                        ResourceList_Day.ToList().ForEach(resource =>
                        {
                            resource.ResourceBgColourBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#846B51");
                        });
                    }

                    if (appointmentData != null && appointmentData.Count() > 0)
                    {
                        appointmentData.ToList().ForEach(x =>
                        {
                            string colorCode = SelectedSchedulerType == SchedulerTypes.RoomView
                                                ? ResourceList_Day.FirstOrDefault(y => y.ResourceId == x.RoomId).AppointmentBgColourCode
                                                : ResourceList_Day.FirstOrDefault(y => y.ResourceId == x.DoctorId).AppointmentBgColourCode;

                            x.AppointmentBackground = (SolidColorBrush)new BrushConverter().ConvertFrom(colorCode);
                            x.StartTime = x.StartTime.ToLocalTime();
                            x.EndTime = x.EndTime.ToLocalTime();
                            x.StartTimeStr = x.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                            x.EndTimeStr = x.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                            x.Duration = x.EndTime.Subtract(x.StartTime).TotalMinutes;
                            x.ResourceIdCollection = new ObservableCollection<object> { SelectedSchedulerType == SchedulerTypes.RoomView ? x.RoomId : x.DoctorId };
                        });

                        AppointmentList = appointmentData.ToObservableCollection();
                    }

                }));

                mainViewModel.CalledMethodCount++;
            });
            task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
        }
        #endregion

        #region Custom Appointment Editor Popup

        #region ***Appointment Calender*** : Commands
        public ICommand AppointmentEditorOpenCommand { get; set; }
        public ICommand AppointmentResizingCommand { get; set; }
        public ICommand AppointmentDroppingCommand { get; set; }

        public ICommand DateNavigationForwardCommand { get; set; }
        public ICommand DateNavigationBackwordCommand { get; set; }
        #endregion

        #region ***Appointment Calender*** : Add/Edit Appointment
        public void OpenAppointmentEditor(AppointmentEditorOpeningEventArgs e)
        {
            try
            {
                if (e.DateTime >= DateTime.Today || e.Appointment != null)
                {
                    IsDatePassed = false;
                    LoadPatients();
                    e.Cancel = true;
                    IsNewAppointment = !(e.Appointment != null);

                    if (SelectedView == CustomSchedulerViewType.Day)
                    {
                        SelectedResource_Day = e.Resource != null
                            ? ResourceList_Day.FirstOrDefault(x => x.ResourceId == (int)e.Resource.Id)
                            : ResourceList_Day.FirstOrDefault();
                    }

                    appointmentEditor = new AppointmentEditor(this);
                    appointmentEditor.Owner = AppStarter.MainWindow;

                    if (e.Appointment != null)
                    {
                        IsDatePassed = e.Appointment.EndTime.Date < DateTime.Today.Date;
                        EditAppointment(e.Appointment as AppointmentModel);
                    }
                    else
                    {
                        Appointment.Id = 0;
                        Appointment.StartTime = e.DateTime;

                        //custom fields
                        Appointment.PatientId = null;
                        Appointment.AppointmentTypeId = null;
                        Appointment.RoomId = null;
                        Appointment.DoctorId = null;

                        Appointment.Notes = null;
                        Appointment.IsRecurrence = false;
                        SelectedRecurrenceMonth = 3;
                        SelectedAppointmentStartTime = SelectedView == CustomSchedulerViewType.Month ? SelectedAppointmentStartTime
                            : AppointmentRefData.GetStartTimeList().OrderBy(x => Math.Abs(x.Key - e.DateTime.TimeOfDay.TotalMinutes)).FirstOrDefault().Key;

                        EndDate = StartDate = e.DateTime > DateTime.Today ? e.DateTime : DateTime.Today;
                        //todo :SchedulerDisplayDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
                        AddAppointment(Appointment);
                    }
                }
                else
                {
                    CustomMessageBox.ShowOK(localisation.Scheduler_PastDaysAppointmentCreationValidation_resx, localisation.AppointmentEditor_NewAppointmentHeader_resx, localisation.OkBtn_resx, MessageBoxImage.Warning);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void AddAppointment(AppointmentModel newappointment)
        {
            SearchedText = string.Empty;

            appointmentEditor.Visibility = Visibility.Visible;
            appointmentEditor.Delete.Visibility = Visibility.Collapsed;
            appointmentEditor.ShowDialog();

            if (isSaveClick && SelectedPatient != null)
            {
                InsertOrUpdateAppointment(newappointment);
            }
            else
                GetAppointmentsAfterCrudOperation();
        }
        private void EditAppointment(AppointmentModel appointment)
        {
            StartDate = appointment.StartTime;
            EndDate = appointment.EndTime;
            DisplayDate = new DateTime(StartDate.Year, StartDate.Month, 1);
            //todo :SchedulerDisplayDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day);
            SearchedText = PatientsList != null ? PatientsList.FirstOrDefault(x => x.ObjectId == appointment.PatientId).PatientName : appointment.PatientName;
            SelectedAppointmentTypeId = appointment.AppointmentTypeId;
            SelectedRoomId = appointment.RoomId;
            SelectedUserId = appointment.DoctorId;
            SelectedAppointmentStartTime = StartDate.TimeOfDay.TotalMinutes;
            SelectedAppointmentEndTime = EndDate.TimeOfDay.TotalMinutes;
            SelectedDuration = appointment.EndTime.Subtract(appointment.StartTime).TotalMinutes;
            SelectedRecurrenceMonth = appointment.RecurrenceMonth > 3 ? appointment.RecurrenceMonth : 3;


            Appointment = appointment;

            appointmentEditor.Visibility = Visibility.Visible;
            appointmentEditor.Delete.Visibility = Visibility.Visible;
            appointmentEditor.ShowDialog();

            if (isSaveClick)
            {
                InsertOrUpdateAppointment(appointment);
            }
            if (isDeleteClick)
                DeleteAppointment((int)appointment.Id, appointment.RecursiveId, appointment.IsFullSeriesAppointmentEdit);
        }
        private void InsertOrUpdateAppointment(AppointmentModel appointment)
        {
            appointment = SetAppointmentData(appointment);
            var task = Task.Factory.StartNew(() => { });
            task.ContinueWith(async (priorTask) =>
            {
                var result = await Task.Run(() => Appointment.InsertOrUpdateAppointment(appointment));
                if (result > 0)
                {
                    if (!AppStarter.IsNavigateToPatientDashboard)
                        GetAppointmentsAfterCrudOperation();
                    else
                        mainViewModel.ReturnBackToView();
                    isSaveClick = false;

                    appointment.Id = result;
                    InsertUpdateCloudSyncData(appointment);
                }
            });
        }
        private void InsertUpdateCloudSyncData(AppointmentModel appointment)
        {
            //Insert CloudSyncInfo for download/upload the images b/w local and AWS by sync job(service)

            #region Cloud Sync Info

            DateTime downLoadDateTime = appointment.StartTime.AddDays(-7);
            DateTime upLoadDateTime = appointment.StartTime.AddDays(7);

            CloudSyncInfoModel cloudSyncInfoModel = new CloudSyncInfoModel();
            cloudSyncInfoModel.Id = IsNewAppointment ? 0 : 101;
            cloudSyncInfoModel.AppointmentId = (int)appointment.Id;
            cloudSyncInfoModel.PatientId = appointment.PatientId.HasValue ? (int)appointment.PatientId : 0;
            cloudSyncInfoModel.DownloadDateTime = appointment.StartTime;
            cloudSyncInfoModel.UploadDateTime = DateTime.Now > upLoadDateTime ? DateTime.Now : upLoadDateTime;
            cloudSyncInfoModel.SyncType = SyncTypes.Automatic.ToString();
            cloudSyncInfoModel.SyncDownloadStatus = 6;
            cloudSyncInfoModel.SyncUploadStatus = 1;
            cloudSyncInfoModel.InsertOrUpdateCloudSyncInfo_sync(cloudSyncInfoModel);
            #endregion
        }
        private AppointmentModel SetAppointmentData(AppointmentModel appointmentModel)
        {
            if (IsNewAppointment)
            {
                var colorCode = SelectedView == CustomSchedulerViewType.Day
                                ? ResourceList_Day.FirstOrDefault(x => x.ResourceId == (SelectedResource_Day.IsRoomResource ? SelectedRoomId : SelectedUserId)).AppointmentBgColourCode
                                : ResourceList.FirstOrDefault(x => x.ResourceId == (SelectedResource.IsRoomResource ? SelectedRoomId : SelectedUserId)).AppointmentBgColourCode;

                appointmentModel.AppointmentBackground = (SolidColorBrush)new BrushConverter().ConvertFrom(colorCode);
            }
            appointmentModel.PatientId = SelectedPatient != null ? SelectedPatient.ObjectId : appointmentModel.PatientId;
            appointmentModel.AppointmentTypeId = SelectedAppointmentTypeId;
            appointmentModel.RoomId = SelectedRoomId;
            appointmentModel.DoctorId = SelectedUserId;
            appointmentModel.StartTime = StartDate.Date.AddMinutes(SelectedAppointmentStartTime);
            appointmentModel.EndTime = appointmentModel.StartTime.AddMinutes(SelectedDuration);
            appointmentModel.ViewType = !IsNewAppointment ? appointmentModel.ViewType : SelectedSchedulerType == SchedulerTypes.RoomView ? localisation.Scheduler_RoomViewTxt_resx : localisation.Scheduler_DoctorViewTxt_resx;
            appointmentModel.RecurrenceMonth = appointmentModel.IsRecurrence ? SelectedRecurrenceMonth : 0;


            if (!appointmentModel.IsFullSeriesAppointmentEdit)
            {
                appointmentModel.IsRecurrence = false;
                appointmentModel.RecursiveId = 0;
            }
            return appointmentModel;
        }
        #endregion

        #region ***Appointment Calender*** : Delete Appointment
        private void DeleteAppointment(int appointmentId, int recursiveId, bool isFullSeriesAppointmentDelete)
        {
            var task = Task.Factory.StartNew(() => { });

            task.ContinueWith(async (priorTask) =>
            {
                var result = await Task.Run(() => Appointment.DeleteAppointment(appointmentId, recursiveId, isFullSeriesAppointmentDelete));
                if (result)
                {
                    if (!AppStarter.IsNavigateToPatientDashboard)
                        GetAppointmentsAfterCrudOperation();
                    else
                        mainViewModel.ReturnBackToView();
                    isDeleteClick = false;
                }
            });
        }
        #endregion

        #region ***Appointment Calender*** : Reschedule Appointment
        private void ResizeAppointment(AppointmentResizingEventArgs e)
        {
            try
            {
                if (e.Action == ResizeAction.Committing)
                {

                    var duration = e.EndTime.TimeOfDay.TotalMinutes - e.StartTime.TimeOfDay.TotalMinutes;
                    var value = 30;
                    if (duration > 30 && duration < 60) value = 30;
                    else if (duration >= 60 && duration < 90) value = 60;
                    else if (duration >= 90 && duration < 120) value = 90;
                    else if (duration > 120) value = 120;

                    SaveResizeAppointment(e, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void SaveResizeAppointment(AppointmentResizingEventArgs e, int restrictedDuration)
        {
            var resizedAppointment = e.Appointment as AppointmentModel;

            var isStartTimeChanged = resizedAppointment.StartTime != e.StartTime;
            var isEndTimeChanged = resizedAppointment.EndTime != e.EndTime;

            MessageBoxResult result = new MessageBoxResult();
            if (resizedAppointment.IsRecurrence)
            {
                result = CustomMessageBox.ShowYesNoCancel(
                                          localisation.Scheduler_RescheduleAppointmentConfirmationMsg_resx,
                                          localisation.Scheduler_RescheduleAppointmentConfirmationTxt_resx,
                                          localisation.AppointmentEditor_RecurrenceEditRadioButton1_resx,
                                          localisation.AppointmentEditor_RecurrenceEditRadioButton2_resx,
                                          localisation.CancelButton_resx,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    resizedAppointment.IsRecurrence = false;
                    resizedAppointment.RecursiveId = 0;
                }
            }
            else
            {
                if (isStartTimeChanged)
                    resizedAppointment.StartTime = e.EndTime.AddMinutes(-restrictedDuration);
                if (isEndTimeChanged)
                    resizedAppointment.EndTime = e.StartTime.AddMinutes(restrictedDuration);

                int id = resizedAppointment.InsertOrUpdateAppointment_sync(resizedAppointment);
                if (id > 0)
                {
                    if (!AppStarter.IsNavigateToPatientDashboard)
                        GetAppointmentsAfterCrudOperation();
                    else
                        mainViewModel.ReturnBackToView();
                }
            }


            if (result == MessageBoxResult.Yes || result == MessageBoxResult.No)
            {
                if (isStartTimeChanged)
                    resizedAppointment.StartTime = e.EndTime.AddMinutes(-restrictedDuration);
                if (isEndTimeChanged)
                    resizedAppointment.EndTime = e.StartTime.AddMinutes(restrictedDuration);

                int id = resizedAppointment.InsertOrUpdateAppointment_sync(resizedAppointment);
                if (id > 0)
                    GetAppointmentsAfterCrudOperation();
            }
            else
                e.CanCommit = false;
        }
        private void DropAppointment(AppointmentDroppingEventArgs e)
        {
            try
            {
                if (e.Appointment != null)
                {
                    if (e.DropTime >= DateTime.Today)
                    {

                        MessageBoxResult result = new MessageBoxResult();
                        var droppedAppointment = e.Appointment as AppointmentModel;

                        var dropTime = AppointmentRefData.GetStartTimeList().OrderBy(x => Math.Abs(x.Key - e.DropTime.TimeOfDay.TotalMinutes)).FirstOrDefault().Key;
                        droppedAppointment.StartTime = e.DropTime.Date.AddMinutes(dropTime);
                        droppedAppointment.EndTime = e.DropTime.Date.AddMinutes(dropTime + droppedAppointment.Duration);

                        //todo :SchedulerDisplayDate = new DateTime(droppedAppointment.StartTime.Year, droppedAppointment.StartTime.Month, droppedAppointment.StartTime.Day);

                        if (droppedAppointment.IsRecurrence)
                        {
                            result = CustomMessageBox.ShowYesNoCancel(
                                          localisation.Scheduler_RescheduleAppointmentConfirmationMsg_resx,
                                          localisation.Scheduler_RescheduleAppointmentConfirmationTxt_resx,
                                          localisation.AppointmentEditor_RecurrenceEditRadioButton1_resx,
                                          localisation.AppointmentEditor_RecurrenceEditRadioButton2_resx,
                                          localisation.CancelButton_resx,
                                          MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                droppedAppointment.IsRecurrence = false;
                                droppedAppointment.RecursiveId = 0;
                            }
                        }
                        else
                        {
                            int id = droppedAppointment.InsertOrUpdateAppointment_sync(droppedAppointment);
                            if (id > 0)
                            {
                                if (!AppStarter.IsNavigateToPatientDashboard)
                                    GetAppointmentsAfterCrudOperation();
                                else
                                    mainViewModel.ReturnBackToView();
                            }
                        }

                        if (result == MessageBoxResult.Yes || result == MessageBoxResult.No)
                        {
                            int id = droppedAppointment.InsertOrUpdateAppointment_sync(droppedAppointment);
                            if (id > 0)
                            {
                                if (!AppStarter.IsNavigateToPatientDashboard)
                                    GetAppointmentsAfterCrudOperation();
                                else
                                    mainViewModel.ReturnBackToView();
                            }
                        }
                        else
                            GetAppointmentsAfterCrudOperation();
                    }
                    else
                    {
                        CustomMessageBox.ShowOK(localisation.Scheduler_PastDaysAppointmentRescheduleValidation_resx,
                                                localisation.Scheduler_RescheduleAppointmentConfirmationTxt_resx,
                                                localisation.OkBtn_resx,
                                                MessageBoxImage.Warning);
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void GetAppointmentsAfterCrudOperation()
        {
            switch (SelectedView)
            {
                case CustomSchedulerViewType.Day:
                    GetResourcesWithAppointments_Day();
                    break;
                default:
                    GetAppointments_WeekOrMonth();
                    break;
            }
        }
        #endregion




        #region ***Appointment Popup*** : Property
        private bool isSaveClick = false;
        private bool isDeleteClick = false;
        private AppointmentEditor appointmentEditor;

        private bool _isDatePassed;
        public bool IsDatePassed
        {
            get { return _isDatePassed; }
            set { _isDatePassed = value; this.OnPropertyChanged(); }
        }

        private bool _isNewAppointment;
        public bool IsNewAppointment
        {
            get { return _isNewAppointment; }
            set { _isNewAppointment = value; this.OnPropertyChanged(); }
        }

        private AppointmentModel _appointment;
        public AppointmentModel Appointment
        {
            get { return _appointment; }
            set
            {
                _appointment = value;
                this.OnPropertyChanged();
            }
        }


        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; OnPropertyChanged(); }
        }

        private int? _selectedAppointmentTypeId;
        public int? SelectedAppointmentTypeId
        {
            get { return _selectedAppointmentTypeId; }
            set { _selectedAppointmentTypeId = value; OnPropertyChanged(); }
        }

        private int? _selectedRoomId;
        public int? SelectedRoomId
        {
            get { return _selectedRoomId; }
            set { _selectedRoomId = value; OnPropertyChanged(); }
        }

        private int? _SelectedUserId;
        public int? SelectedUserId
        {
            get { return _SelectedUserId; }
            set { _SelectedUserId = value; OnPropertyChanged(); }
        }


        private double _selectedAppointmentStartTime;
        public double SelectedAppointmentStartTime
        {
            get { return _selectedAppointmentStartTime; }
            set { _selectedAppointmentStartTime = value; OnPropertyChanged(); }
        }

        private double _selectedAppointmentEndTime;
        public double SelectedAppointmentEndTime
        {
            get { return _selectedAppointmentEndTime; }
            set { _selectedAppointmentEndTime = value; OnPropertyChanged(); }
        }


        private double _selectedDuration;
        public double SelectedDuration
        {
            get { return _selectedDuration; }
            set { _selectedDuration = value; OnPropertyChanged(); }
        }

        private int _selectedRecurrenceMonth = 6;
        public int SelectedRecurrenceMonth
        {
            get { return _selectedRecurrenceMonth; }
            set { _selectedRecurrenceMonth = value; OnPropertyChanged(); }
        }

        #endregion

        #region ***Appointment Popup*** : Commands

        public ICommand SaveCommand { get; set; }
        public void OnSave()
        {
            IsAutoCompletorPopupOpen = false;
            if (!AppointmentList.Contains(_appointment))
            {
                AppointmentList.Add(_appointment);
            }
            isSaveClick = true;
            isDeleteClick = false;
            appointmentEditor.Close();
            ///Added for the cloud sync 
            if (Appointment.StartTime.Day == DateTime.Now.Day)
            {
                var downloadExePath = ConfigurationManager.AppSettings["DownloadWorkerServicePath"];
                if (File.Exists(downloadExePath))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = downloadExePath;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    // Optionally redirect input, output, and error streams
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    // Start the process
                    process.Start();
                }
                else
                {
                    CustomMessageBox.ShowOK("Download sync application not installed on this system, Please Install.", "Warning!", "Ok", MessageBoxImage.Warning);
                }
            }
        }

        public ICommand CancelCommand { get; set; }
        public void OnCancel()
        {
            isSaveClick = false;
            isDeleteClick = false;
            appointmentEditor.Close();
            IsAutoCompletorPopupOpen = false;
            mainViewModel.ReturnBackToView();
        }

        public ICommand DeleteCommand { get; set; }
        public void OnDelete()
        {
            if (_appointment != null)
            {
                AppointmentList.Remove(_appointment);
                isDeleteClick = true;
                isSaveClick = false;
            }
            IsAutoCompletorPopupOpen = false;
            appointmentEditor.Close();
        }

        public ICommand ClosingCommand { get; set; }
        public void Closing(CancelEventArgs e)
        {
            e.Cancel = true;
            if (IsCalendarStartDatePopUpOpen)
                IsCalendarStartDatePopUpOpen = false;
            appointmentEditor.Visibility = Visibility.Hidden;
            Appointment = Appointment;
        }

        #endregion


        #region ***Appointment Popup*** :  StartDate-DatePicker Control (Property)

        private DateTime _displayDate = DateTime.Today;
        public DateTime DisplayDate
        {
            get { return _displayDate; }
            set { _displayDate = value; OnPropertyChanged(); }
        }


        private bool _isCalendarStartDatePopUpOpen;
        public bool IsCalendarStartDatePopUpOpen
        {
            get { return _isCalendarStartDatePopUpOpen; }
            set { _isCalendarStartDatePopUpOpen = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                EndDate = StartDate;
                if (IsCalendarStartDatePopUpOpen)
                    IsCalendarStartDatePopUpOpen = false;

                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                if (IsCalendarStartDatePopUpOpen)
                    IsCalendarStartDatePopUpOpen = false;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ***Appointment Popup*** :  StartDate-DatePicker Control (Commands)
        public ICommand OpenCalendarCommand { get; set; }
        private void OpenCalendar(object sender)
        {
            if (IsCalendarStartDatePopUpOpen)
                IsCalendarStartDatePopUpOpen = false;

            else if (!IsCalendarStartDatePopUpOpen)
                IsCalendarStartDatePopUpOpen = true;
        }
        #endregion


        #region ***Appointment Popup*** : Patient-AutoComplete Control (Property)

        private bool _isAutoCompletorPopupOpen;
        public bool IsAutoCompletorPopupOpen
        {
            get { return _isAutoCompletorPopupOpen; }
            set { _isAutoCompletorPopupOpen = value; OnPropertyChanged(); }
        }

        private Visibility _isAutoCompletorPopupVisible;
        public Visibility IsAutoCompletorPopupVisible
        {
            get { return _isAutoCompletorPopupVisible; }
            set { _isAutoCompletorPopupVisible = value; OnPropertyChanged(); }
        }

        private string _searchedText;
        public string SearchedText
        {
            get { return _searchedText; }
            set { _searchedText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Patient> _patientsList;
        public ObservableCollection<Patient> PatientsList
        {
            get { return _patientsList; }
            set { _patientsList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Patient> _searchedPatientList;
        public ObservableCollection<Patient> SearchedPatientList
        {
            get { return _searchedPatientList; }
            set { _searchedPatientList = value; OnPropertyChanged(); }
        }

        #endregion

        #region ***Appointment Popup*** : Patient-AutoComplete Control (Commands)
        public ICommand DocumentType_LostFocusCommand { get; set; }
        public ICommand DocumentType_KeyUpCommand { get; set; }
        public ICommand AutoCompletorListSelectionChangedCommand { get; set; }
        #endregion

        #region ***Appointment Popup*** : Patient-AutoComplete Control (Event/Methods)
        private void LoadPatients()
        {
            try
            {
                var task = Task.Factory.StartNew(() => { });
                task.ContinueWith(async (priorTask) =>
                {
                    var patientData = await Appointment.GetPatientsInformation();
                    if (patientData != null && patientData.Count() > 0)
                    {
                        SearchedPatientList = PatientsList = patientData.ToObservableCollection();
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void autoCompletorList_SelectionChanged(object sender)
        {
            try
            {
                if (sender != null)
                {
                    SelectedPatient = (Patient)sender;
                    if (SelectedPatient != null)
                    {
                        IsAutoCompletorPopupOpen = false;
                        SearchedText = SelectedPatient.PatientName;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                LogHelper.LogError(ex);
            }
        }
        private void DocumentType_LostFocus(object sender)
        {
            try
            {
                SelectedPatient = sender != null ? (Patient)sender : null;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void DocumentType_KeyUp(object sender)
        {
            try
            {
                if (sender != null)
                {
                    string searchstring = (string)sender;
                    if (!string.IsNullOrEmpty(searchstring.Trim()))
                    {
                        IsAutoCompletorPopupOpen = true;
                        if (searchstring.Length == 1 || SearchedPatientList.Count() == 0 || PreviousSearchedString.Contains(searchstring))
                            SearchedPatientList = SearchOperation(PatientsList, searchstring);
                        else
                            SearchedPatientList = SearchOperation(PatientsList, searchstring);

                        PreviousSearchedString = searchstring;
                    }
                    IsAutoCompletorPopupOpen = searchstring.Length == 0 ? false : true;
                }
                else
                {
                    IsAutoCompletorPopupOpen = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private ObservableCollection<Patient> SearchOperation(ObservableCollection<Patient> searchList, string searchedString)
        {
            SearchedPatientList = searchList.Where(p => (p.PatientName.Trim().ToLower().Contains(searchedString.Trim().ToLower()))).ToObservableCollection();
            if (SearchedPatientList.Count() == 0)
                SearchedPatientList = searchList.Where(p => (p.MiddleName.Trim().ToLower().Contains(searchedString.Trim().ToLower()))).ToObservableCollection();
            if (SearchedPatientList.Count() == 0)
                SearchedPatientList = searchList.Where(p => (p.LastName.Trim().ToLower().Contains(searchedString.Trim().ToLower()))).ToObservableCollection();
            if (SearchedPatientList.Count() == 0)
                SearchedPatientList = searchList.Where(p => (p.PatientRecordnumber.Trim().ToLower().Contains(searchedString.Trim().ToLower()))).ToObservableCollection();
            return SearchedPatientList;
        }
        #endregion

        #endregion


    }
}
