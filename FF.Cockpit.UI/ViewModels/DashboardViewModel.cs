using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.Views.UserControls.Dashboard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using localisation = FF.Cockpit.Common.Properties.Resources;
using MessageBox = System.Windows.Forms.MessageBox;

namespace FF.Cockpit.UI.ViewModels
{
    public class DashboardViewModel : PropertyChangeHelper
    {
        
        public MainViewModel mainViewModel { get; set; }
        private ExcisedMarkerListTile excisedMarkerListTile;

        #region Constructor
        public DashboardViewModel()
        {
            try
            {
                if (ConfigurationManager.AppSettings["DashboardYearFilter"] != null)
                    FilterYearFRomConfig = Convert.ToInt32(ConfigurationManager.AppSettings["DashboardYearFilter"]);

                mainViewModel = (MainViewModel)AppStarter.mainVM;

                //patients list commands
                PatientListSelectionChangedCommand = new RelayCommandHelper(x => PatientSelectedChanged(x));
                ClearSelectedPatientInfoCommand = new RelayCommandHelper(x => ClearSelectedPatientInfo(x));
                HiddenTextboxWatermarkCommand = new RelayCommandHelper(x => HiddenTextboxWatermark(x));
                //PatientSearchTextCommand = new RelayCommandHelper(x => PatientSearchText(x));
                AutoCompletorListSelectionChangedCommand = new RelayCommandHelper(x => autoCompletorList_SelectionChanged(x));
                //DocumentType_LostFocusCommand = new RelayCommandHelper(x => DocumentType_LostFocus(x));
                DocumentType_KeyUpCommand = new RelayCommandHelper(x => DocumentType_KeyUp(x));

                //Excision List Commands
                ExcisedMarkerTileData_Results = new ObservableCollection<DashboardExcisedMarkerTileData_Result>();
                GetExcisedMarkersListCommand = new RelayCommandHelper(x => OpenExcisionListPopup(x));

                //dashboard commands
                DashboardFilterCommand = new RelayCommandHelper(x => DashboardDataFilter(x));
                SelectedFollowUpSessionCommand = new RelayCommandHelper(x => GetSelectedFollowUpSessionData(x));
                BackToOperationDashboardCommand = new RelayCommandHelper(x => mainViewModel.ReturnBackToView());

                EditMedicalHistoryCommand = new RelayCommandHelper(x => { EditMedicalHistory(); });
                SaveMedicalHistoryCommand = new RelayCommandHelper(x => { SaveMedicalHistory(); });
                ChangeAppointmentCommand = new RelayCommandHelper(x => { ChangeAppointMentMethod(); });
                ClosePatientCommand = new RelayCommandHelper(x => { ClosePatient(); });
                EditSkinTypeCommand = new RelayCommandHelper(x => { EditSkinType(); });
                SelectedSkintileCommand = new RelayCommandHelper(x => SelectedSkinType(x));
                SaveSkinTypeCommand = new RelayCommandHelper(x => SaveSkinType());
                MedicalHistorySkinCancerAnalyzedCommand = new RelayCommandHelper(x => SkinCancerAnalysis(x));
                MedicalHistoryCancerRiskAnalyzedCommand = new RelayCommandHelper(x => CancerAnalysis(x));
                MedicalHistoryGeneticCancerAnalyzedCommand = new RelayCommandHelper(x => GeneticConsumptionAnalysis(x));
                if (AppStarter.IsNavigateToPatientDashboard)
                    LoadDashboardUC();
                else
                    LoadPatientUC();

                IsBackButtonVisible = AppStarter.IsNavigateFromOperationalDashboard || AppStarter.IsNavigateFromPerformanceDashboard ? Visibility.Visible : Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #endregion

        #region DashBoard Tiles

        #region Properties 

        private int filterYearFRomConfig;
        public int FilterYearFRomConfig
        {
            get { return filterYearFRomConfig; }
            set { filterYearFRomConfig = value; OnPropertyChanged(); }
        }

        private int selectedFilterYear;
        public int SelectedFilterYear
        {
            get { return selectedFilterYear; }
            set { selectedFilterYear = value; OnPropertyChanged(); }
        }
        private DateTime? selectedFilterDate;
        public DateTime? SelectedFilterDate
        {
            get { return selectedFilterDate; }
            set { selectedFilterDate = value; OnPropertyChanged(); }
        }
        private ObservableCollection<DashboardExcisedMarkerTileData_Result> excisedMarkerTileData_Results;
        public ObservableCollection<DashboardExcisedMarkerTileData_Result> ExcisedMarkerTileData_Results
        {
            get { return excisedMarkerTileData_Results; }
            set { excisedMarkerTileData_Results = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ExaminationHistoryData_Result> examinationHistoryData_ResultList;
        public ObservableCollection<ExaminationHistoryData_Result> ExaminationHistoryData_ResultList
        {
            get { return examinationHistoryData_ResultList; }
            set
            {

                examinationHistoryData_ResultList = value; if (value != null && value.Count > 0)
                {
                    var AllExsionCountsExceptZero = ExaminationHistoryData_ResultList.Where(x => x.ExcisionCount > 0).ToList();
                    if (AllExsionCountsExceptZero.Count > 0)
                        DashboardTilesData_ResultObj.ExcisionCount = AllExsionCountsExceptZero[0].ExcisionCount;
                }
                OnPropertyChanged();
            }
        }
        private SessionIntervalTileData_Result sessionIntervalTileData_ResultObj;
        public SessionIntervalTileData_Result SessionIntervalTileData_ResultObj
        {
            get { return sessionIntervalTileData_ResultObj; }
            set { sessionIntervalTileData_ResultObj = value; OnPropertyChanged(); }
        }
        private FollowUpSessionTileModel followUpSessionTileModelObj;
        public FollowUpSessionTileModel FollowUpSessionTileModelObj
        {
            get { return followUpSessionTileModelObj; }
            set { followUpSessionTileModelObj = value; OnPropertyChanged(); }
        }

        private DashboardTilesData_Result dashboardTilesData_ResultObj;
        public DashboardTilesData_Result DashboardTilesData_ResultObj
        {
            get { return dashboardTilesData_ResultObj; }
            set { dashboardTilesData_ResultObj = value; OnPropertyChanged(); }
        }
        private bool isLoaderActiveOnMedicalHistory;
        public bool IsLoaderActiveOnMedicalHistory
        {
            get { return isLoaderActiveOnMedicalHistory; }
            set { isLoaderActiveOnMedicalHistory = value; OnPropertyChanged(); }
        }

        private bool isLoaderActiveOnSkinType;
        public bool IsLoaderActiveOnSkinType
        {
            get { return isLoaderActiveOnSkinType; }
            set { isLoaderActiveOnSkinType = value; OnPropertyChanged(); }
        }


        private bool isLoaderActiveOnFollowUpSession;
        public bool IsLoaderActiveOnFollowUpSession
        {
            get { return isLoaderActiveOnFollowUpSession; }
            set { isLoaderActiveOnFollowUpSession = value; OnPropertyChanged(); }
        }
        #endregion

        #region Dashboard userControl

        private Visibility isDashboardUCVisible;
        public Visibility IsDashboardUCVisible
        {
            get { return isDashboardUCVisible; }
            set { isDashboardUCVisible = value; OnPropertyChanged(); }
        }

        #endregion

        #region MedicalHistory Tiles Properties

        private MedicalHistoryTileData_Result medicalHistoryTileModelObj;
        public MedicalHistoryTileData_Result MedicalHistoryTileModelObj
        {
            get { return medicalHistoryTileModelObj; }
            set { medicalHistoryTileModelObj = value; OnPropertyChanged(); }
        }

        private MedicalHistoryTileData_Result previous_MedicalHistoryTileModelObj;

        private Visibility isEditButtonVisible;
        public Visibility IsEditButtonVisible
        {
            get { return isEditButtonVisible; }
            set { isEditButtonVisible = value; OnPropertyChanged(); }
        }

        private Visibility isSaveButtonVisible;
        public Visibility IsSaveButtonVisible
        {
            get { return isSaveButtonVisible; }
            set { isSaveButtonVisible = value; OnPropertyChanged(); }
        }

        private bool isEditModeEnabled;
        public bool IsEditModeEnabled
        {
            get { return isEditModeEnabled; }
            set { isEditModeEnabled = value; OnPropertyChanged(); }
        }
        #endregion

        #region Skin Tile Propertes
        private SkinTileContentModel skinTileContentModelObj;
        public SkinTileContentModel SkinTileContentModelObj
        {
            get { return skinTileContentModelObj; }
            set { skinTileContentModelObj = value; OnPropertyChanged(); }
        }

        private bool isskinTileLoadMode;
        public bool IsskinTileLoadMode
        {
            get { return isskinTileLoadMode; }
            set { isskinTileLoadMode = value; OnPropertyChanged(); }
        }

        private bool isskinTileEditMode;
        public bool IsskinTileEditMode
        {
            get { return isskinTileEditMode; }
            set { isskinTileEditMode = value; OnPropertyChanged(); }
        }

        private bool isskinTileSelected;
        public bool IsskinTileSelected
        {
            get { return isskinTileSelected; }
            set { isskinTileSelected = value; OnPropertyChanged(); }
        }

        private bool isskinTileSaveMode;
        public bool IsskinTileSaveMode
        {
            get { return isskinTileSaveMode; }
            set { isskinTileSaveMode = value; OnPropertyChanged(); }
        }

        private bool isSkinTileEmpty;
        public bool IsSkinTileEmpty
        {
            get { return isSkinTileEmpty; }
            set { isSkinTileEmpty = value; OnPropertyChanged(); }
        }
        private Visibility isBackButtonVisible;
        public Visibility IsBackButtonVisible
        {
            get { return isBackButtonVisible; }
            set { isBackButtonVisible = value; OnPropertyChanged(); }
        }

        private Visibility isSkinTileEditButtonVisible;
        public Visibility IsSkinTileEditButtonVisible
        {
            get { return isSkinTileEditButtonVisible; }
            set { isSkinTileEditButtonVisible = value; OnPropertyChanged(); }
        }

        private Visibility isSkinTileSaveButtonVisible;
        public Visibility IsSkinTileSaveButtonVisible
        {
            get { return isSkinTileSaveButtonVisible; }
            set { isSkinTileSaveButtonVisible = value; OnPropertyChanged(); }
        }

        private Visibility isSkinTileMissingTextVisible;
        public Visibility IsSkinTileMissingTextVisible
        {
            get { return isSkinTileMissingTextVisible; }
            set { isSkinTileMissingTextVisible = value; OnPropertyChanged(); }
        }


        #endregion

        #region Command

        public RelayCommandHelper ClosePatientCommand { get; private set; }
        public RelayCommandHelper ChangeAppointmentCommand { get; private set; }
        public RelayCommandHelper EditMedicalHistoryCommand { get; private set; }
        public RelayCommandHelper SaveMedicalHistoryCommand { get; private set; }
        public RelayCommandHelper EditSkinTypeCommand { get; private set; }
        public RelayCommandHelper SaveSkinTypeCommand { get; private set; }
        public RelayCommandHelper MedicalHistorySkinCancerAnalyzedCommand { get; private set; }
        public RelayCommandHelper MedicalHistoryCancerRiskAnalyzedCommand { get; private set; }
        public RelayCommandHelper MedicalHistoryGeneticCancerAnalyzedCommand { get; private set; }
        public RelayCommandHelper SelectedSkintileCommand { get; private set; }
        public RelayCommandHelper SelectedFollowUpSessionCommand { get; private set; }
        public RelayCommandHelper DashboardFilterCommand { get; private set; }
        public RelayCommandHelper BackToOperationDashboardCommand { get; private set; }
        #endregion

        #region Method

        public void LoadDashboardUC()
        {
            DispatcherOperation op = App.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                IsEditButtonVisible = Visibility.Visible;
                IsSaveButtonVisible = Visibility.Collapsed;
                IsEditModeEnabled = false;
                IsSkinTileEditButtonVisible = Visibility.Visible;
                IsSkinTileSaveButtonVisible = Visibility.Collapsed;
                IsSkinTileMissingTextVisible = Visibility.Collapsed;
                IsskinTileLoadMode = true;
                IsskinTileEditMode = false;

            }));

            if (AppStarter.SelectedPatientId > 0)
            {

                var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());

                task.ContinueWith(async (priorTask) =>
                {
                    // DOING SOME HEAVY LIFTING WORK HERE
                    await Task.Run(() => GetDashboardCountData());
                    await Task.Run(() => GetExaminationHistoryData());
                    await Task.Run(() => GetFollowUpSessionData());
                    await Task.Run(() => GetSessionIntervalData());
                    await Task.Run(() => GetMedicalHistory());

                    // TRYING TO FORCE THE UI TO REFRESH
                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {

                    }));
                });


                task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
            }
        }

        public void DashboardDataFilter(object selectionType)
        {
            if (selectionType != null)
            {
                if (Convert.ToInt32(selectionType) == FilterYearFRomConfig)
                    SelectedFilterDate = DateTime.Now.AddYears(-FilterYearFRomConfig).Date;
                else
                    SelectedFilterDate = null;
                SelectedFilterYear = Convert.ToInt32(selectionType);
                LoadDashboardUC();
            }
        }

        private async void GetExaminationHistoryData()
        {
            mainViewModel.TotalCallMethodCount++;
            var ExaminationHistoryData = await new ExaminationHistoryData_Result().GetExaminationHistoryTileData(AppStarter.SelectedPatientId, SelectedFilterDate);
            //Task.Delay(10000);

            if (ExaminationHistoryData != null && ExaminationHistoryData.Count() > 0)
            {

                ExaminationHistoryData_ResultList = new ObservableCollection<ExaminationHistoryData_Result>();
                ExaminationHistoryData_ResultList = ExaminationHistoryData.ToObservableCollection();

                int numberOfDays = 0;
                if (ExaminationHistoryData_ResultList.Count() > 1)
                {
                    for (int i = 1; i < ExaminationHistoryData_ResultList.Count(); i++)
                    {
                        numberOfDays += Convert.ToInt32((Convert.ToDateTime(ExaminationHistoryData_ResultList[i - 1].SessionDate) - (Convert.ToDateTime(ExaminationHistoryData_ResultList[i].SessionDate))).TotalDays);
                    }
                }
                var AllBodyScanCountsExceptZero = ExaminationHistoryData_ResultList.Where(x => x.BodyScanCount > 0).ToList();
                if (AllBodyScanCountsExceptZero.Count > 0)
                    DashboardTilesData_ResultObj.LesionsCount = Math.Round(AllBodyScanCountsExceptZero.Average(item => item.BodyScanCount));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    List<int> markers = ExaminationHistoryData_ResultList.Select(x => x.MarkerCount).ToList();
                    DashboardTilesData_ResultObj.LesionsUnderFollowUpCount = markers.Sum() - ((int)DashboardTilesData_ResultObj.ExcisionCount - (int)DashboardTilesData_ResultObj.GhostMarkersCount - (int)DashboardTilesData_ResultObj.DeactivatedMarkers);
                    DashboardTilesData_ResultObj.LesionsUnderFollowUpCount = DashboardTilesData_ResultObj.LesionsUnderFollowUpCount > 0 ? DashboardTilesData_ResultObj.LesionsUnderFollowUpCount : 0;

                    var sessionscount = ExaminationHistoryData_ResultList.Select(x => x.SessionNumber).ToList().Count();
                    DashboardTilesData_ResultObj.SessionCount = sessionscount;
                });



                ////average 
                var sessionCount = ExaminationHistoryData_ResultList.Count > 1 ? ExaminationHistoryData_ResultList.Count - 1 : 1;
                double calculatedDays = numberOfDays / sessionCount;
                string timeString = localisation.NotApplicableStr_resx;
                double years = calculatedDays / 365;
                double remainingDays = calculatedDays % 365;
                double month = calculatedDays / 30;
                double weeks = remainingDays / 7;
                double days = remainingDays % 7;

                if (calculatedDays > 0 && calculatedDays <= 7)
                    timeString = days + " " + localisation.DayStr_resx;
                if (calculatedDays > 7 && calculatedDays <= 30)
                    timeString = Math.Round(weeks, 1) + localisation.WeekStr_resx;
                if (calculatedDays >= 31 && calculatedDays < 365 && month != 0)
                    timeString = Math.Round(month, 1) + localisation.MonthStr_resx;
                if (calculatedDays > 365 && years != 0)
                    timeString = Math.Round(years, 1) + localisation.YearStr_resx;
                if (SessionIntervalTileData_ResultObj == null)
                    SessionIntervalTileData_ResultObj = new SessionIntervalTileData_Result();
                SessionIntervalTileData_ResultObj.Interval = timeString.ToString();
            }
            mainViewModel.CalledMethodCount++;
        }
        private async void GetSessionIntervalData()
        {
            try
            {
                mainViewModel.TotalCallMethodCount++;
                SessionIntervalTileData_ResultObj = new SessionIntervalTileData_Result();
                var sessionIntervalTileData = await SessionIntervalTileData_ResultObj.GetSessionIntervalTileData(AppStarter.SelectedPatientId, SelectedFilterDate);
                if (sessionIntervalTileData != null)
                {
                    var appointmentStartAt = sessionIntervalTileData.AppointmentStartAt.ToLocalTime();
                    var appointmentEndAt = sessionIntervalTileData.AppointmentEndAt.ToLocalTime();
                    var dataString= $"{appointmentStartAt.ToString("dd.MM.yyyy hh:mm tt", CultureInfo.InvariantCulture)} : {appointmentEndAt.ToString("dd.MM.yyyy hh:mm tt", CultureInfo.InvariantCulture)}";
                    SessionIntervalTileData_ResultObj.NextAppointmentDate = dataString.ToString();
                    SessionIntervalTileData_ResultObj.FrequencyLevel= sessionIntervalTileData.FrequencyLevel;
                }
                mainViewModel.CalledMethodCount++;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }


        }
        private async void GetFollowUpSessionData()
        {
            mainViewModel.TotalCallMethodCount++;
            var FollowUpSessionTileData = await new FollowUpSessionTileModel().GetFolloUpSessionTileData(AppStarter.SelectedPatientId, SelectedFilterDate);
            if (FollowUpSessionTileData != null)
            {
                FollowUpSessionTileModelObj = FollowUpSessionTileData;

            }
            mainViewModel.CalledMethodCount++;
        }
        private void GetSelectedFollowUpSessionData(object selectedSessionYear)
        {
            if (selectedSessionYear != null)
            {
                FollowUpSessionTileModelObj.SelectedSessionYear = (SessionYearsData_Result)selectedSessionYear;

                var task = Task.Factory.StartNew(() => IsLoaderActiveOnFollowUpSession = true);

                task.ContinueWith(async (priorTask) =>
                {
                    await FollowUpSessionTileModelObj.SetChartData();
                });
                task.ContinueWith((antecedent) => Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    App.Current.Dispatcher.Invoke(new Action(() => IsLoaderActiveOnFollowUpSession = false), DispatcherPriority.ContextIdle, null);
                }));
            }
        }
        private async void GetDashboardCountData()
        {
            mainViewModel.TotalCallMethodCount++;
            DashboardTilesData_ResultObj = new DashboardTilesData_Result();
            var DashboardTilesData = await DashboardTilesData_ResultObj.GetDashboardTilesData(AppStarter.SelectedPatientId, SelectedFilterDate);
            if (DashboardTilesData != null)
            {
                var startTime = DashboardTilesData.AppointmentStartDate?.ToLocalTime();
                var endTime = DashboardTilesData.AppointmentEndDate?.ToLocalTime();
                if (startTime != null && endTime != null)
                    DashboardTilesData.NextAppointmentDate = startTime?.ToString(AppStarter.sysCultureInfo.DateTimeFormat.ShortDatePattern + " " + AppStarter.sysCultureInfo.DateTimeFormat.ShortTimePattern) + " - " + endTime?.ToString(AppStarter.sysCultureInfo.DateTimeFormat.ShortDatePattern + " " + AppStarter.sysCultureInfo.DateTimeFormat.ShortTimePattern);
                DashboardTilesData_ResultObj = DashboardTilesData;
                if (DashboardTilesData_ResultObj.SkinTypeId >= 0)
                {
                    SkinTileContentModelObj = new SkinTileContentModel();
                    SkinTileContentModelObj.OrderSkinTiles(DashboardTilesData_ResultObj.SkinTypeId);
                }

            }
            mainViewModel.CalledMethodCount++;
        }

        private async Task UpdateSkinType()
        {
            var result = await Task.Run(() => DashboardTilesData_ResultObj.UpdateSkinTileData(AppStarter.SelectedPatientId, Convert.ToInt16(DashboardTilesData_ResultObj.SkinTypeId)));
        }

        private async void GetMedicalHistory()
        {
            mainViewModel.TotalCallMethodCount++;
            previous_MedicalHistoryTileModelObj = new MedicalHistoryTileData_Result();
            MedicalHistoryTileModelObj = new MedicalHistoryTileData_Result();
            MedicalHistoryTileModelObj.LastUpdatedVisible = Visibility.Collapsed;
            var MedicalHistoryTileData = await new MedicalHistoryTileData_Result().GetMedicalHistoryByPatientId(AppStarter.SelectedPatientId);
            if (MedicalHistoryTileData != null)
            {
                MedicalHistoryTileModelObj = MedicalHistoryTileData;

                previous_MedicalHistoryTileModelObj.IsSkinCancerAnalysed = MedicalHistoryTileModelObj.IsSkinCancerAnalysed;
                previous_MedicalHistoryTileModelObj.IsCancerAnalysed = MedicalHistoryTileModelObj.IsCancerAnalysed;
                previous_MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed = MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed;
                if (!string.IsNullOrEmpty(MedicalHistoryTileModelObj.LastUpdatedOn))
                {
                    MedicalHistoryTileModelObj.LastUpdatedVisible = Visibility.Visible;
                    DateTime _dateTime = DateTime.Parse(MedicalHistoryTileModelObj.LastUpdatedOn, CultureInfo.InvariantCulture);
                    MedicalHistoryTileModelObj.LastUpdatedOn = _dateTime.ToString(ConstantHelper.DateFormat);
                    MedicalHistoryTileModelObj.SelectedskinCancerOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(MedicalHistoryTileModelObj.IsSkinCancerAnalysed);
                    MedicalHistoryTileModelObj.SelectedCancerOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(MedicalHistoryTileModelObj.IsCancerAnalysed);
                    MedicalHistoryTileModelObj.SelectedgeneticalConspicuousOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed);
                }
                else
                    MedicalHistoryTileModelObj.LastUpdatedVisible = Visibility.Collapsed;
            }
            else
            {
                MedicalHistoryTileModelObj.SelectedskinCancerOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(false);
                MedicalHistoryTileModelObj.SelectedCancerOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(false);
                MedicalHistoryTileModelObj.SelectedgeneticalConspicuousOption = MedicalHistoryTileModelObj.GetSelectedComboboxItem(false);
            }
            mainViewModel.CalledMethodCount++;
        }

        private void EditMedicalHistory()
        {
            IsEditButtonVisible = Visibility.Collapsed;
            IsSaveButtonVisible = Visibility.Visible;
            IsEditModeEnabled = true;

        }
        private void SkinCancerAnalysis(object selectedValue)
        {
            if (selectedValue != null)
            {
                MedicalHistoryTileModelObj.IsSkinCancerAnalysed = Convert.ToBoolean(selectedValue);
            }
        }
        private void CancerAnalysis(object selectedValue)
        {
            if (selectedValue != null)
            {
                MedicalHistoryTileModelObj.IsCancerAnalysed = Convert.ToBoolean(selectedValue);
            }
        }

        private void GeneticConsumptionAnalysis(object selectedValue)
        {
            if (selectedValue != null)
            {
                MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed = Convert.ToBoolean(selectedValue);
            }
        }



        private void SaveMedicalHistory()
        {

            var task = Task.Factory.StartNew(() => IsLoaderActiveOnMedicalHistory = true);
            task.ContinueWith(async (priorTask) =>
            {
                if (IsSaveButtonVisible == Visibility.Visible)
                {
                    if (!CheckExistingData())
                    {
                        MedicalHistoryTileModelObj.PatientId = AppStarter.SelectedPatientId;
                        MedicalHistoryTileModelObj.LastUpdatedOn = DateTime.Now.ToString(ConstantHelper.DateFormat);
                        await InsertMedicalHistory();
                        GetMedicalHistory();
                    }

                    IsEditButtonVisible = Visibility.Visible;
                    IsSaveButtonVisible = Visibility.Collapsed;
                    IsEditModeEnabled = false;
                }
            });
            task.ContinueWith((antecedent) => Task.Run(async () =>
            {
                await Task.Delay(1000);
                App.Current.Dispatcher.Invoke(new Action(() => IsLoaderActiveOnMedicalHistory = false), DispatcherPriority.ContextIdle, null);
            }));
        }
        private async Task InsertMedicalHistory()
        {
            var result = await Task.Run(() => MedicalHistoryTileModelObj.InsertMedicalHistoryByPatientId(MedicalHistoryTileModelObj));
        }
        public void ClosePatient()
        {
            SelectedPatient = null;
            SelectedFilterDate = null;  
            IsPatientUCVisible = Visibility.Visible;
            IsDashboardUCVisible = Visibility.Collapsed;

            IsSearchTextBoxVisible = Visibility.Visible;
            TbxSearchPatientWatermark = Visibility.Visible;
            SearchedText = string.Empty;
            if (PatientsDetails_ResultObj == null)
                GetPatientDetails();
            LoadDashboardUC();
            AppStarter.IsNavigateFromOperationalDashboard = false;
            AppStarter.IsNavigateFromPerformanceDashboard = false;


        }
        public void ChangeAppointMentMethod()
        {
            try
            {
                if (mainViewModel != null)
                {
                    AppStarter.IsNavigateToPatientDashboard = false;
                    mainViewModel.SelectedMenu = mainViewModel.NavigationModelList[1];
           
                }
                //var SessionIntervalTileData = SessionIntervalTileData_ResultObj.UpdateSessionIntervalTileData(AppStarter.SelectedPatientId, SessionIntervalTileData_ResultObj.AppointmentDate);
                //int isUpdate = SessionIntervalTileData.Result;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public bool CheckExistingData()
        {

            if (MedicalHistoryTileModelObj.LastUpdatedOn != null && previous_MedicalHistoryTileModelObj.IsSkinCancerAnalysed == MedicalHistoryTileModelObj.IsSkinCancerAnalysed &&
                       previous_MedicalHistoryTileModelObj.IsCancerAnalysed == MedicalHistoryTileModelObj.IsCancerAnalysed &&
                       previous_MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed == MedicalHistoryTileModelObj.IsGeneticalConspicuousAnalsyed)
                return true;
            else
                return false;
        }
        private void EditSkinType()
        {
            DialogWindowHelper dialogWindow = new DialogWindowHelper();

            bool result = dialogWindow.ShowPopupMessage(localisation.Warning_resx, localisation.SkinTileImportantNotice_resx, localisation.MessageConfirmTemplateName, localisation.SkinTileUpdateConfirmation_resx, true, true);
            if (result)
            {
                IsskinTileEditMode = true;
                IsskinTileLoadMode = false;
                IsskinTileSelected = false;
                IsSkinTileEmpty = false;
                IsskinTileSaveMode = false;
                IsSkinTileSaveButtonVisible = Visibility.Visible;
                IsSkinTileEditButtonVisible = Visibility.Collapsed;
            }
        }
        private void SelectedSkinType(object selectedTileId)
        {
            int intNumeral = 0;
            switch (selectedTileId.ToString())
            {
                case "I": intNumeral = 1; break;
                case "II": intNumeral = 2; break;
                case "III": intNumeral = 3; break;
                case "IV": intNumeral = 4; break;
                case "V": intNumeral = 5; break;
            }
            IsskinTileSelected = true;

            dashboardTilesData_ResultObj.SkinTypeId = Convert.ToInt16(intNumeral);
            if (IsSkinTileEmpty && dashboardTilesData_ResultObj.SkinTypeId >= 0)
            {
                IsSkinTileEmpty = false;
                IsskinTileLoadMode = true;
            }
        }
        private void SaveSkinType()
        {
            IsskinTileEditMode = false;
            IsskinTileLoadMode = false;
            IsskinTileSelected = false;
            IsSkinTileEmpty = false;
            IsskinTileSaveMode = false;

            if (dashboardTilesData_ResultObj.SkinTypeId <= 0)
            {
                IsSkinTileMissingTextVisible = Visibility.Visible;

                IsskinTileEditMode = false;
                IsskinTileLoadMode = false;
                IsskinTileSelected = false;
                IsSkinTileEmpty = true;
                IsskinTileSaveMode = true;
            }
            else
            {
                var task = Task.Factory.StartNew(() => IsLoaderActiveOnSkinType = true);

                task.ContinueWith(async (priorTask) =>
                {
                    IsSkinTileEmpty = false;

                    await UpdateSkinType();
                    GetDashboardCountData();

                    IsSkinTileMissingTextVisible = Visibility.Collapsed;
                    IsSkinTileEditButtonVisible = Visibility.Visible;
                    IsSkinTileEditButtonVisible = Visibility.Visible;
                    IsSkinTileSaveButtonVisible = Visibility.Collapsed;

                    IsskinTileEditMode = false;
                    IsskinTileSelected = false;
                    IsskinTileSaveMode = false;
                    IsskinTileLoadMode = true;
                });
                task.ContinueWith((antecedent) => Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    App.Current.Dispatcher.Invoke(new Action(() => IsLoaderActiveOnSkinType = false), DispatcherPriority.ContextIdle, null);
                }));
            }
        }
        #endregion

        #endregion

        #region Patient UC

        #region pateint UC Visibility
        private Visibility isPatientUCVisible;
        public Visibility IsPatientUCVisible
        {
            get { return isPatientUCVisible; }
            set { isPatientUCVisible = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties     

        private int totalPatientCount;
        public int TotalPatientCount
        {
            get { return totalPatientCount; }
            set { totalPatientCount = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// ///////////////Property to handle the UI of DataGrid
        /// </summary>
        private ObservableCollection<Patient> patientsDetails_Result;
        public ObservableCollection<Patient> PatientsDetails_Result
        {
            get { return patientsDetails_Result; }
            set { patientsDetails_Result = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// ///////////////Property to get the data from Db
        /// </summary>
        private ObservableCollection<Patient> patientsDetails_ResultObj;
        public ObservableCollection<Patient> PatientsDetails_ResultObj
        {
            get { return patientsDetails_ResultObj; }
            set { patientsDetails_ResultObj = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Patient> patientsDetails_ResultAutoCompleteObj;
        public ObservableCollection<Patient> PatientsDetails_ResultAutoCompleteObj
        {
            get { return patientsDetails_ResultAutoCompleteObj; }
            set { patientsDetails_ResultAutoCompleteObj = value; OnPropertyChanged(); }
        }

        private Patient selectedPatient;
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
            set { selectedPatient = value; OnPropertyChanged(); }
        }
        private string searchedText;
        public string SearchedText
        {
            get { return searchedText; }
            set { searchedText = value; OnPropertyChanged(); }
        }

        private Visibility isSearchTextBoxVisible;
        public Visibility IsSearchTextBoxVisible
        {
            get { return isSearchTextBoxVisible; }
            set { isSearchTextBoxVisible = value; OnPropertyChanged(); }
        }


        private Visibility tbxSearchPatientWatermark;
        public Visibility TbxSearchPatientWatermark
        {
            get { return tbxSearchPatientWatermark; }
            set { tbxSearchPatientWatermark = value; OnPropertyChanged(); }
        }
        private Visibility tbxSearchPatient;
        public Visibility TbxSearchPatient
        {
            get { return tbxSearchPatient; }
            set { tbxSearchPatient = value; OnPropertyChanged(); }
        }

        private Visibility isAutoCompletorPopupVisible;
        public Visibility IsAutoCompletorPopupVisible
        {
            get { return isAutoCompletorPopupVisible; }
            set { isAutoCompletorPopupVisible = value; OnPropertyChanged(); }
        }

        private bool isAutoCompletorPopupOpen;
        public bool IsAutoCompletorPopupOpen
        {
            get { return isAutoCompletorPopupOpen; }
            set { isAutoCompletorPopupOpen = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public RelayCommandHelper PatientListSelectionChangedCommand { get; private set; }
        public RelayCommandHelper GetExcisedMarkersListCommand { get; private set; }
        public RelayCommandHelper ClearSelectedPatientInfoCommand { get; private set; }
        public RelayCommandHelper HiddenTextboxWatermarkCommand { get; private set; }
        public RelayCommandHelper PatientSearchTextCommand { get; private set; }
        public RelayCommandHelper AutoCompletorListSelectionChangedCommand { get; private set; }
        public RelayCommandHelper DocumentType_KeyUpCommand { get; private set; }
        public RelayCommandHelper DocumentType_LostFocusCommand { get; private set; }

        #endregion

        #region Methods
        private async void GetPatientDetails()
        {
            mainViewModel.TotalCallMethodCount++;
            var PatientsDetails = await new Patient().GetPatients();
            if (PatientsDetails != null)
            {
                PatientsDetails_Result = PatientsDetails.ToObservableCollection();
                TotalPatientCount = PatientsDetails_Result.Count();

            }
            mainViewModel.CalledMethodCount++;
        }

        public void LoadPatientUC()
        {
            try
            {
                IsPatientUCVisible = Visibility.Visible;
                IsDashboardUCVisible = Visibility.Collapsed;
                var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());
                task.ContinueWith(async (priorTask) =>
                {

                    // DOING SOME HEAVY LIFTING WORK HERE
                    await Task.Run(() => GetPatientDetails());
                    // TRYING TO FORCE THE UI TO REFRESH
                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {

                    }));
                });

                task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

        }
        public void PatientSelectedChanged(object selectedPatientId)
        {
            if (selectedPatientId != null)
            {
                AppStarter.SelectedPatientId = (int)selectedPatientId;

                if (AppStarter.SelectedPatientId > 0)
                {
                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        IsSearchTextBoxVisible = Visibility.Collapsed;
                        IsPatientUCVisible = Visibility.Collapsed;
                        IsDashboardUCVisible = Visibility.Visible;
                        IsAutoCompletorPopupOpen = false;
                    }));

                    DashboardDataFilter(SelectedFilterYear);
                }
                else
                {

                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        IsSearchTextBoxVisible = Visibility.Visible;
                        SearchedText = string.Empty;
                        TbxSearchPatientWatermark = Visibility.Visible;

                    }));
                }
            }
        }
        private void ClearSelectedPatientInfo(object x)
        {
            IsSearchTextBoxVisible = Visibility.Visible;
            SearchedText = string.Empty;
            TbxSearchPatientWatermark = Visibility.Visible;
            patientsDetails_Result = PatientsDetails_ResultObj;
            PatientsDetails_ResultAutoCompleteObj = PatientsDetails_ResultObj;
        }
        private void HiddenTextboxWatermark(object x)
        {
            TbxSearchPatientWatermark = Visibility.Collapsed;
            TbxSearchPatient = Visibility.Visible;
        }
        private void PatientSearchText(object searchText)
        {
            DispatcherOperation op = App.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                string searchString = (string)searchText;
                if (!string.IsNullOrEmpty(searchString))
                {
                    TbxSearchPatientWatermark = Visibility.Collapsed;
                    TbxSearchPatient = Visibility.Visible;
                }
                else
                {
                    SearchedText = string.Empty;
                    TbxSearchPatient = Visibility.Collapsed;
                    TbxSearchPatientWatermark = Visibility.Visible;
                }
            }));

        }
        private void autoCompletorList_SelectionChanged(object sender)
        {
            try
            {
                if (sender != null)
                {
                    var autoCompletorList = (Patient)sender;
                    if (autoCompletorList != null)
                    {
                        IsAutoCompletorPopupOpen = false;
                        AppStarter.SelectedPatientId = autoCompletorList.ObjectId.Value;
                        IsSearchTextBoxVisible = Visibility.Collapsed;
                        IsPatientUCVisible = Visibility.Collapsed;
                        IsDashboardUCVisible = Visibility.Visible;
                        IsAutoCompletorPopupVisible = Visibility.Collapsed;
                        DashboardDataFilter(SelectedFilterYear);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.LogError(ex);
            }
        }

        private void OpenExcisionListPopup(object isExcisedMarkersListButtonClick)
        {
            excisedMarkerListTile = new ExcisedMarkerListTile(this);
            GetExcisedMarkerList();

            if (ExcisedMarkerTileData_Results.Count > 0)
            {
                excisedMarkerListTile.ShowDialog();
            }
            else
            {
                CustomMessageBox.ShowOK(localisation.NoRecordFoundMsg_resx, " ", localisation.OkBtn_resx, MessageBoxImage.Information);
            }
        }


        private void GetExcisedMarkerList()
        {
            var excisedMarkerResult = new DashboardExcisedMarkerTileData_Result();
            try
            {
                if (mainViewModel != null)
                {
                    mainViewModel.TotalCallMethodCount++;
                    var ExcisedMarkerDetails = excisedMarkerResult.GetExcisedMarkerList(AppStarter.SelectedPatientId, SelectedFilterDate);

                    if (ExcisedMarkerDetails != null)

                        ExcisedMarkerTileData_Results = ExcisedMarkerDetails.ToObservableCollection();
                    mainViewModel.CalledMethodCount++;

                }
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
                TbxSearchPatientWatermark = Visibility.Collapsed;
                if (sender != null)
                {
                    string searchstring = (string)sender;
                    if (!string.IsNullOrEmpty(searchstring.Trim()) && searchstring.Length >= 1)
                    {
                        IsAutoCompletorPopupOpen = true;
                        IsAutoCompletorPopupVisible = Visibility.Visible;
                       PatientsDetails_ResultObj = PatientsDetails_Result.
                                Where(pateint => (pateint.FirstName.Trim().ToLower().Contains(searchstring.Trim().ToLower())
                                               || pateint.MiddleName.Trim().ToLower().Contains(searchstring.Trim().ToLower())
                                               || pateint.LastName.Trim().ToLower().Contains(searchstring.Trim().ToLower())
                                               || pateint.PatientRecordnumber.Trim().ToLower().Contains(searchstring.Trim().ToLower())
                                               || (pateint.FirstName.Trim() + pateint.MiddleName.Trim() + pateint.LastName.Trim()).ToString().ToLower().Contains(searchstring.Trim().ToLower())
                                               || (pateint.FirstName.Trim() +" "+ pateint.MiddleName.Trim() + " " + pateint.LastName.Trim()).ToString().ToLower().Contains(searchstring.Trim().ToLower())
                                               )).ToObservableCollection();
                        PatientsDetails_ResultAutoCompleteObj = PatientsDetails_ResultObj;

                    }
                    else
                        CloseAutoPopup();
                }
                else
                    CloseAutoPopup();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.LogError(ex);
            }
        }
        private void CloseAutoPopup()
        {
            if (PatientsDetails_ResultObj != null)
                PatientsDetails_ResultObj.Clear();
            IsAutoCompletorPopupOpen = false;
            IsAutoCompletorPopupVisible = Visibility.Collapsed;
        }
        
        #endregion Methods

        #endregion

    }
}