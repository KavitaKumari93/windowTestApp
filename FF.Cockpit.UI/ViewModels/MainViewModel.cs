using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.Views.UserControls.CloudSync;
using FF.Cockpit.UI.Views.UserControls.LeftBottomPanel;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class MainViewModel : PropertyChangeHelper
    {
        private BackgroundWorker backgroundWorker = null;
        public CockpitSettingsModel cockpitSettingsModel = null;
        public ObservableCollection<DataGridSettings> DataGridSettingList { get; set; } = new ObservableCollection<DataGridSettings>();
        #region Properties 
        public int TotalCallMethodCount { get; set; }
        private int dataLoader;
        public int CalledMethodCount
        {
            get { return dataLoader; }
            set { dataLoader = value; OnPropertyChanged(); LoaderHide(TotalCallMethodCount); }
        }

        private string _fullVersion = ConstantHelper.CockpitFullVersion;
        public string FullVersion
        {
            get { return _fullVersion; }
        }

        private string _shortVersion = ConstantHelper.CockpitShortVersion;
        public string ShortVersion
        {
            get { return _shortVersion; }
        }

        private NavigationResponse selectedMenu;
        public NavigationResponse SelectedMenu
        {
            get { return selectedMenu; }
            set
            {
                selectedMenu = value;
                if (selectedMenu != null)
                {
                    if (!AppStarter.IsNavigateToPatientDashboard)
                        ShowRightPanel();
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NavigationResponse> navigationModelList = null;
        public ObservableCollection<NavigationResponse> NavigationModelList
        {
            get { return navigationModelList; }
            set { navigationModelList = value; OnPropertyChanged(); }
        }

        private bool isLoaderActive;
        public bool IsLoaderActive
        {
            get { return isLoaderActive; }
            set { isLoaderActive = value; OnPropertyChanged(); }
        }
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set { currentView = value; OnPropertyChanged(); }
        }
        private DateTime? _currentDate = null;
        public DateTime? CurrentDate
        {
            get { return _currentDate; }
            set { _currentDate = value; OnPropertyChanged(); }
        }
        private string _syncStatus;
        public string SyncStatus
        {
            get { return _syncStatus; }
            set { _syncStatus = value; OnPropertyChanged(); }
        }
        private bool _isImageRotate;
        public bool IsImageRotate
        {
            get { return _isImageRotate; }
            set { _isImageRotate = value; OnPropertyChanged(); }
        }

        #endregion

        #region Constructor
        public MainViewModel()
        {
            try
            {
                backgroundWorker = new BackgroundWorker();
                cockpitSettingsModel = new CockpitSettingsModel();
                SyncCommand = new RelayCommandHelper(x => { SyncDumpData(); });
                AccountCommand = new RelayCommandHelper(x => { Account(); });
                SwitchAccountCommand = new RelayCommandHelper(x => { SwitchAccount(); });
                LogoutCommand = new RelayCommandHelper(x => { Logout(); });
                NavigationModelList = new NavigationResponse().GetNavigationList().Where(x => x.IsActive == true).ToObservableCollection();
                AboutCommand = new RelayCommandHelper(x => { About(); });
                TermsCommand = new RelayCommandHelper(x => { TermsOfServices(); });
                PrivacyCommand = new RelayCommandHelper(x => { Privacy(); });
                ContactCommand = new RelayCommandHelper(x => { Contacts(); });
                SelectedMenu = NavigationModelList[2];
                BindModules();
                #region Background Events
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(DoWork_SyncDumpData);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BackgroundWorker_ProgressChanged);
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncCompleted);
                #endregion


            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #endregion

        #region Commands
        public RelayCommandHelper AccountCommand { get; private set; }
        public RelayCommandHelper SwitchAccountCommand { get; private set; }
        public RelayCommandHelper LogoutCommand { get; private set; }
        public RelayCommandHelper AboutCommand { get; private set; }
        public RelayCommandHelper TermsCommand { get; private set; }
        public RelayCommandHelper PrivacyCommand { get; private set; }
        public RelayCommandHelper ContactCommand { get; private set; }
        public RelayCommandHelper SyncCommand { get; private set; }
        #endregion

        #region Methods
        private void About()
        {
            try
            {
                CurrentView = new AboutViewModel();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void TermsOfServices()
        {
            try
            {
                CurrentView = new TermsofServiceUC();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void Privacy()
        {
            try
            {
                CurrentView = new PrivacyUC();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void Contacts()
        {
            try
            {
                CurrentView = new ContactUC();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        private void Account()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void SwitchAccount()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private void Logout()
        {
            try
            {
                DialogWindowHelper dialogWindow = new DialogWindowHelper();
                bool result = dialogWindow.ShowPopupMessage(localisation.DialogWindowAlertCaption_resx, string.Empty, string.Empty, localisation.LogOutStr_resx, true, true);
                if (result)
                {
                    // Close the current window
                    AppStarter.MainWindow.Close();
                    // Exit the current application process
                    Application.Current.Shutdown();
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public ObservableCollection<DataGridSettings> GetDatagridSettings()
        {
            var DataGridSettingData = new DataGridSettings().GetDataGridSettings();
            if (DataGridSettingData != null && DataGridSettingData.Count() > 0)
                DataGridSettingList = DataGridSettingData.ToObservableCollection();
            return DataGridSettingList;
        }


        private void ShowRightPanel()
        {
            try
            {
                GetDatagridSettings();
                var DataGridSettingData = new DataGridSettings().GetDataGridSettings();
                if (DataGridSettingData != null && DataGridSettingData.Count() > 0)
                    DataGridSettingList = DataGridSettingData.ToObservableCollection();

                SetLoaderCountZero();

                //close calender popup
                if (AppStarter.IsPerformanceCalendarPopUpOpen != null && AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen)
                {
                    AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen = false;
                }
                if (AppStarter.IsSchedulerCalendarPopUpOpen != null && AppStarter.IsSchedulerCalendarPopUpOpen.IsOpen)
                {
                    AppStarter.IsSchedulerCalendarPopUpOpen.IsOpen = false;
                }
                AppStarter.IsNavigateFromPerformanceDashboard = false;
                AppStarter.IsNavigateFromOperationalDashboard = false;
                AppStarter.mainVM = this;
                BindSelectedView();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public void BindSelectedView()
        {
            GetCockpitSettings();
            if (SelectedMenu.Id != 0)
            {
                switch (SelectedMenu.Id)
                {
                    case 1:
                        CurrentView = new OperationalViewModel();
                        break;
                    case 2:
                        CurrentView = new SchedulerViewModel();
                        break;
                    case 3:
                        CurrentView = new DashboardViewModel();
                        break;
                    case 4:
                        CurrentView = new PerformanceViewModel();
                        break;
                    case 5:
                        CurrentView = new ServiceViewModel();
                        break;
                    case 6:
                        CurrentView = new ConfigurationViewModel();
                        break;
                    case 7:
                        CurrentView = new CloudSyncViewModel();
                        break;
                    case 8:
                        CurrentView = new TrendsGraphViewModel();
                        break;
                    default:
                        CurrentView = new DashboardViewModel();
                        break;
                }
            }
        }

        public void SetLoaderCountZero()
        {
            TotalCallMethodCount = 0;
            CalledMethodCount = 0;
        }
        public void LoaderShow()
        {
            IsLoaderActive = true;
        }

        public void LoaderHide(int numberofCounts)
        {
            if (CalledMethodCount == numberofCounts && TotalCallMethodCount != 0)
            {
                IsLoaderActive = false;
                TotalCallMethodCount = 0;
                CalledMethodCount = 0;
            }
        }

        #endregion

        public void ReturnBackToView()
        {
            App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if (AppStarter.IsNavigateFromOperationalDashboard)
                {
                    SelectedMenu = NavigationModelList[0];
                    CurrentView = new OperationalViewModel();
                }
                if (AppStarter.IsNavigateFromPerformanceDashboard)
                {
                    SelectedMenu = NavigationModelList[3];
                    CurrentView = new PerformanceViewModel();
                }

            }));
            AppStarter.IsNavigateToPatientDashboard = false;
            AppStarter.IsNavigateFromPerformanceDashboard = false;
            AppStarter.IsNavigateFromOperationalDashboard = false;

        }
        private void SyncDumpData()
        {
            try
            {

                if (backgroundWorker.IsBusy)
                    return;
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public void DoWork_SyncDumpData(object sender, DoWorkEventArgs e)
        {
            IsImageRotate = true;
            BackgroundWorker bw = sender as BackgroundWorker;
            if (sender != backgroundWorker) return;
            bw.ReportProgress(2);
            new Patient().SyncPatientDumpData();
            new HighRiskPatientTileData_Result().SyncHighRiskPatientDumpData();
            bw.ReportProgress(100);
            SyncStatus = localisation.SyncLastRefreshStr_resx;
        }
        public void SyncCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (sender != backgroundWorker) return;
            DateTime lastSync = DateTime.Now;
            cockpitSettingsModel.InsertOrUpdateCockpitSettingInfo(lastSync);
            SyncStatus = localisation.SyncLastRefreshStr_resx;
            CurrentDate = lastSync;
            IsImageRotate = false;
            //To reload the current binded view so that it will refresh the dump data on pressing the Refresh Button.
            BindSelectedView();
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SyncStatus = localisation.SyncInprogressStr_resx;
            CurrentDate = null;
        }
        public void GetCockpitSettings()
        {
            try
            {
                //get Last refereh Date from db
                var settings = cockpitSettingsModel.GetCockpitSettingInfo();
                SyncStatus = settings != null ? localisation.SyncLastRefreshStr_resx : localisation.SyncNeverRefreshStr_resx;
                CurrentDate = settings != null ? settings.LastSyncedOn : CurrentDate;

                //get Miscellaneous settings from db whether patient name should be anonymized or not
                MiscellaneousSettingsModel miscellaneousModel = new MiscellaneousSettingsModel();
                var data = new MiscellaneousSettingsModel().GetMiscellaneousData_sync();
                if (data != null && data.Count() > 0)
                {
                    AppStarter.IsPatientNameAnontmized = data.SingleOrDefault(x => x.Name == nameof(MiscellaneousKeys.IsPatientsNameAnonymized)) == null || data.FirstOrDefault(x => x.Name == nameof(MiscellaneousKeys.IsPatientsNameAnonymized)).Value?.ToString() == "0" ? false : true;
                   // AppStarter.IsServiceTabVisible = data.SingleOrDefault(x => x.Name == nameof(MiscellaneousKeys.IsServiceMenuVisible)) == null || data.FirstOrDefault(x => x.Name == nameof(MiscellaneousKeys.IsServiceMenuVisible)).Value?.ToString() == "0" ? false : true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        public void BindModules()
        {
            if (AppointmentRefData.miscellaneousData != null)
            {
                if (AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsCloudSyncMenuVisible.ToString())?.Value == null || AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsCloudSyncMenuVisible.ToString())?.Value.ToString() == "0")
                {
                    var delobj = NavigationModelList.Where(x => x.Id == (int)ModuleNames.CloudSync).FirstOrDefault();
                    NavigationModelList.Remove(delobj);
                }
                if (AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsServiceMenuVisible.ToString())?.Value == null || AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsServiceMenuVisible.ToString())?.Value.ToString() == "0")
                {
                    var obj = NavigationModelList.Where(x => x.Id == (int)ModuleNames.Service).FirstOrDefault();
                    NavigationModelList.Remove(obj);

                }
                else
                {
                    if (NavigationModelList.FirstOrDefault(x => x.Name == ModuleNames.Service.ToString()) == null)
                        NavigationModelList.Add(new NavigationResponse() { Id = (int)ModuleNames.Service, Name = localisation.Service_resx, IconKey = "Icon_Service", IsSelected = false, IsActive = true });

                }

            }

        }
    }
}
