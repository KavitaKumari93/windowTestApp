using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Views.UserControls.Performance;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Application = System.Windows.Application;
using DataGrid = System.Windows.Controls.DataGrid;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class PerformanceViewModel : PropertyChangeHelper
    {
        MainViewModel mainViewModel { get; set; }
        private LowFrequentPatients lowFrequentPatients;

        #region Properties  
        private string _lowFrequencyStr;
        public string LowFrequencyStr
        {
            get { return _lowFrequencyStr; }
            set { _lowFrequencyStr = value; OnPropertyChanged(); }
        }

        private bool _isShowListOfPatients;
        public bool IsShowListOfPatients
        {
            get { return _isShowListOfPatients; }
            set { _isShowListOfPatients = value; OnPropertyChanged(); }
        }
        private bool _isDataGridExpand = false;
        public bool IsDataGridExpand
        {
            get { return _isDataGridExpand; }
            set { _isDataGridExpand = value; OnPropertyChanged(); }
        }

        private object _lockMutex = new object();

        private PerformanceLowFrequentPatientsData_Result _selectedPatient;
        public PerformanceLowFrequentPatientsData_Result SelectedPatient
        {
            get { return _selectedPatient; }
            set { _selectedPatient = value; }
        }
        /// <summary>
        /// ///////////////Property to get the data from Db
        /// </summary>
        private HighRiskPatientTileData_Result _selectedHighRiskPatient;
        public HighRiskPatientTileData_Result SelectedHighRiskPatient
        {
            get { return _selectedHighRiskPatient; }
            set { _selectedHighRiskPatient = value; }
        }
        /// <summary>
        /// ///////////////Property to get the data  of  HighRiskPatientTileData_Result from Db
        /// </summary>
        private ObservableCollection<PerformanceLowFrequentPatientsData_Result> lowFrequentpatientsDetails_Result;
        public ObservableCollection<PerformanceLowFrequentPatientsData_Result> LowFrequentpatientsDetails_Result
        {
            get { return lowFrequentpatientsDetails_Result; }
            set { lowFrequentpatientsDetails_Result = value; OnPropertyChanged(); }
        }
        private ObservableCollection<HighRiskPatientTileData_Result> highRiskPatientTileData_ResultListObj;
        public ObservableCollection<HighRiskPatientTileData_Result> HighRiskPatientTileData_ResultListObj
        {
            get { return highRiskPatientTileData_ResultListObj; }
            set { highRiskPatientTileData_ResultListObj = value; OnPropertyChanged(); }
        }
        private ObservableCollection<HighRiskPatientTileData_Result> highRiskPatientTileData_ResultList;
        public ObservableCollection<HighRiskPatientTileData_Result> HighRiskPatientTileData_ResultList
        {
            get { return highRiskPatientTileData_ResultList; }
            set { highRiskPatientTileData_ResultList = value; OnPropertyChanged(); }
        }
        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; OnPropertyChanged(); }
        }
        private string dateString;
        public string DateString
        {
            get { return dateString; }
            set { dateString = value; OnPropertyChanged(); }
        }
        public bool setOnce { get; set; }
        private DateTime? fromDate = DateTime.Today;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged(); }
        }

        private DateTime toDate = DateTime.Today;
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; OnPropertyChanged(); }

        }
        private DateTime? previousFromDate = DateTime.Now;
        public DateTime? PreviousFromDate
        {
            get { return previousFromDate; }
            set { previousFromDate = value; OnPropertyChanged(); }
        }

        private DateTime previousToDate = DateTime.Now;
        public DateTime PreviousToDate
        {
            get { return previousToDate; }
            set { previousToDate = value; OnPropertyChanged(); }
        }

        private DateTime currentDate = DateTime.Today;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; OnPropertyChanged(); }
        }
        private DateTime datePickerStartDate = Convert.ToDateTime(localisation.PerformanceFilterDefaultStartDate_resx);
        public DateTime DatePickerStartDate
        {
            get { return datePickerStartDate; }
            set { datePickerStartDate = value; OnPropertyChanged(); }
        }

        private string customStartDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
        public string CustomStartDate
        {
            get { return customStartDate; }
            set { customStartDate = value; OnPropertyChanged(); }
        }
        private DateTime customSelectedDate;
        public DateTime CustomSelectedDate
        {
            get { return customSelectedDate; }
            set { customSelectedDate = value; SelctedDate(); OnPropertyChanged(); }
        }
        private string customEndDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
        public string CustomEndDate
        {
            get { return customEndDate; }
            set { customEndDate = value; OnPropertyChanged(); }
        }
        private string selectedFilter;
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set { selectedFilter = value; OnPropertyChanged(); }
        }

        private bool isCalendarPopUpOpen;
        public bool IsCalendarPopUpOpen
        {
            get { return isCalendarPopUpOpen; }
            set { isCalendarPopUpOpen = value; OnPropertyChanged(); }
        }
        private Visibility genericCustomButtonVisibility;
        public Visibility GenericCustomButtonVisibility
        {
            get { return genericCustomButtonVisibility; }
            set { genericCustomButtonVisibility = value; OnPropertyChanged(); }
        }

        private Visibility customDateTextboxVisibility;
        public Visibility CustomDateTextboxVisibility
        {
            get { return customDateTextboxVisibility; }
            set { customDateTextboxVisibility = value; OnPropertyChanged(); }
        }


        private PerformanceTilesData_Result performanceTilesData_ResultObj;
        public PerformanceTilesData_Result PerformanceTilesData_ResultObj
        {
            get { return performanceTilesData_ResultObj; }
            set { performanceTilesData_ResultObj = value; OnPropertyChanged(); }
        }
        private FilterType peformanceMeasuresFilterObj = 0;
        public FilterType PeformanceMeasuresFilterObj
        {
            get { return peformanceMeasuresFilterObj; }
            set { peformanceMeasuresFilterObj = value; OnPropertyChanged(); }
        }
        private int _pageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; OnPropertyChanged(); }
        }

        #endregion

        #region Constructor
        public PerformanceViewModel()
        {
            // Get the mainViewModel from AppStarter
            mainViewModel = (MainViewModel)AppStarter.mainVM;
            // Initialize various commands

            var expandValue = GetDataGridSettingModel();
            if (expandValue != null)
                IsDataGridExpand = Convert.ToInt32(expandValue.GridConfigValue) == 0 ? false : true;

            ExportCSVCommand = new RelayCommandHelper(x => ExportCSVMethod(x));
            ExportPDFCommand = new RelayCommandHelper(x => ExportPDFMethod(x));
            GridExpandCommand = new RelayCommandHelper(x => DataGridExpandMethod());
            PerformanceFilterSelectionCommand = new RelayCommandHelper(x => FilterSelection(x));
            NavigateToPatientDashBoardCommand = new RelayCommandHelper(x => NavigateToPatientDashboard(x));
            NavigateToPatientDashboardCommand = new RelayCommandHelper(x => NavigateToPatientDashboard(x));
            GetLowFrequentPatientListCommand = new RelayCommandHelper(x => OpenPatientListPopup(x));
            ClickCommand = new RelayCommandHelper(x=> MouseClick());
            SelectedDateCommand = new RelayCommandHelper(x => SelctedDate());
            DatePickerCommand = new RelayCommandHelper(x => DatePickerClick());
            // PreviousPageCommand = new RelayCommandHelper(x => PreviousPage());
            // NextPageCommand = new RelayCommandHelper(x => NextPage());
            PerformanceTilesData_ResultObj = new PerformanceTilesData_Result();
            LowFrequentpatientsDetails_Result = new ObservableCollection<PerformanceLowFrequentPatientsData_Result>();
            BindingOperations.EnableCollectionSynchronization(LowFrequentpatientsDetails_Result, _lockMutex);
            HighRiskPatientTileData_ResultList = new ObservableCollection<HighRiskPatientTileData_Result>();
            //Just ask the binding to enable synching of the collection
            BindingOperations.EnableCollectionSynchronization(HighRiskPatientTileData_ResultList, _lockMutex);
            DateString = localisation.StartDateStr_resx;
            SelectedFilter = FilterType.FULLVIEW.ToString();
            GenericCustomButtonVisibility = Visibility.Visible;
            CustomDateTextboxVisibility = Visibility.Collapsed;
            FilterSelection(SelectedFilter);
        }
        #endregion

        #region Commands
        // Command to Export the Grid in CSV.
        public RelayCommandHelper ExportCSVCommand { get; private set; }

        // Command to Expand the Grid.
        public RelayCommandHelper ExportPDFCommand { get; private set; }

        // Command to Expand the Grid.
        public RelayCommandHelper GridExpandCommand { get; private set; }
        // Command to get LowFrequentPatient List.
        public RelayCommandHelper GetLowFrequentPatientListCommand { get; private set; }
        // Command to Select the PerformanceFilter.
        public RelayCommandHelper PerformanceFilterSelectionCommand { get; private set; }
        // Command For ClaendarControl.
        public RelayCommandHelper CalendarCommand { get; private set; }
        // Command to select date from calendar control.
        public RelayCommandHelper SelectedDateCommand { get; private set; }
        // Command For Datepicker.
        public RelayCommandHelper DatePickerCommand { get; private set; }
        // Command For close the calendar popup.
        public RelayCommandHelper ClosePopUpCommand { get; private set; }
        // Command For navigation of performance dashboard to PatientDashboard..
        public RelayCommandHelper NavigateToPatientDashboardCommand { get; private set; }
        public RelayCommandHelper NavigateToPatientDashBoardCommand { get; private set; }
        public RelayCommandHelper ClickCommand{ get; private set; }
        #endregion

        #region Methods


        #region Export

        // method to export the datagrid in csv
        private void ExportCSVMethod(object grd)
        {
            string filePath = Extension.GetFilePath(ModuleNames.Performance.ToString(), false);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());
                task.ContinueWith(async (priorTask) =>
                {
                    // DOING SOME HEAVY LIFTING WORK HERE
                    await Task.Run(() => ExportCSV(grd, filePath));
                });
                task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
            }
        }
        private async Task ExportCSV(object grd, string filePath)
        {
            mainViewModel.TotalCallMethodCount++;
            try
            {
                if (grd != null)
                {
                    System.Windows.Controls.DataGrid dataGrid = (System.Windows.Controls.DataGrid)grd;
                    var list = new List<HighRiskPatientTileData_Result>(dataGrid.ItemsSource as IEnumerable<HighRiskPatientTileData_Result>);
                    var dataTable = ToDataTable(list);
                    if (dataTable != null)
                    {
                        Thread.Sleep(1000);
                        await Extension.ExportToCSV(dataTable, filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            mainViewModel.CalledMethodCount++;
        }


        // method to export the datagrid in pdf
        private void ExportPDFMethod(object grd)
        {
            string filePath = Extension.GetFilePath(ModuleNames.Performance.ToString(),true);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());
                task.ContinueWith(async (priorTask) =>
                {
                    // DOING SOME HEAVY LIFTING WORK HERE
                    await Task.Run(() => ExportPDF(grd, filePath));
                });
                task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
            }
        }
        private async Task ExportPDF(object grd, string filePath)
        {
            mainViewModel.TotalCallMethodCount++;
            try
            {
                if (grd != null)
                {
                    DataGrid dataGrid = (DataGrid)grd;
                    var list = new List<HighRiskPatientTileData_Result>(dataGrid.ItemsSource as IEnumerable<HighRiskPatientTileData_Result>);
                    var dataTable = ToDataTable(list);
                    if (dataTable != null)
                    {
                        Thread.Sleep(1000);
                        await Extension.ExportToPDF(dataTable, filePath, localisation.HiskRiskPatientHeader_resx);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            mainViewModel.CalledMethodCount++;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Props = Props.Where(p => p.Name != "PatientId").ToArray();

            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion


        // method to open PatientList Popup
        private void OpenPatientListPopup(object isGetLowFrequentPatientListButtonClick)
        {
            try
            {
                lowFrequentPatients = new LowFrequentPatients(this);
                GetPatientDetails(); 

                if (LowFrequentpatientsDetails_Result.Count > 0)
                {
                    lowFrequentPatients.ShowDialog();
                }
                else
                {
                    CustomMessageBox.ShowOK(localisation.NoRecordFoundMsg_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {

                LogHelper.LogError(ex);
            }

        }
        // method to navigate from performance dashboard to PatientDashboard.
        private void NavigateToPatientDashboard(object patientId)
        {
            try
            {
                if (mainViewModel != null && patientId != null)
                {
                    if (lowFrequentPatients != null)
                        lowFrequentPatients.Close();

                    //Navigate to Patient Dashboard
                    AppStarter.IsNavigateToPatientDashboard = true;
                    AppStarter.IsNavigateFromPerformanceDashboard = true;
                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        mainViewModel.SelectedMenu = mainViewModel.NavigationModelList[2];
                        mainViewModel.CurrentView = new DashboardViewModel();
                    }));
                    var dashboardViewModel = (DashboardViewModel)mainViewModel.CurrentView;
                    dashboardViewModel.PatientSelectedChanged((int)patientId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        // method to get the LowFrequentPatient List
        private void GetPatientDetails()
        {
            var patientmodel = new PerformanceLowFrequentPatientsData_Result();
            try
            {
                if (mainViewModel != null)
                {
                    mainViewModel.TotalCallMethodCount++;
                    var PatientsDetails = patientmodel.GetLowFrequentPatient(fromDate, toDate);
                    if (PatientsDetails != null)
                    {
                        LowFrequentpatientsDetails_Result = PatientsDetails.ToObservableCollection();
                    }
                }
                mainViewModel.CalledMethodCount++;

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #region Exapnd settings

        // method to expand the datagrid
        private void DataGridExpandMethod()
        {
            IsDataGridExpand = !IsDataGridExpand;
            SaveGridExpandCollapse(GetDataGridSettingModel());
        }
        private DataGridSettings SaveGridExpandCollapse(DataGridSettings model)
        {
            if (model == null)
            {
                model = new DataGridSettings();
                model.Id = 0;
                model.UserId = 0;
                model.ModuleId = (int)ModuleNames.Performance;
                model.GridName = DataGridNames.HighRiskPatient.ToString();
                model.GridConfigName = DataGridConfigName.ExpandCollapse.ToString();
                model.GridConfigValue = IsDataGridExpand ? "1" : "0";
            }
            else
                model.GridConfigValue = IsDataGridExpand ? "1" : "0";

            model.InsertOrUpdateDataGridSettings(model);

            var data = new DataGridSettings().GetDataGridSettings();
            if (data != null)
                mainViewModel.DataGridSettingList = data.ToObservableCollection();

            return model;
        }

        private DataGridSettings GetDataGridSettingModel()
        {
            return mainViewModel.DataGridSettingList.Where(x => x.UserId == 0
            && x.ModuleId == (int)ModuleNames.Performance
            && x.GridName == DataGridNames.HighRiskPatient.ToString()
            && x.GridConfigName == DataGridConfigName.ExpandCollapse.ToString()).FirstOrDefault();
        }
        #endregion

        // method to select various selection filter
        public void FilterSelection(object selectionType)
        {
            try
            {
                LowFrequencyStr = localisation.LowFrequencyDefaultStr_resx;
                if (selectionType != null)
                {
                    FilterType peformance;
                    Enum.TryParse<FilterType>((string)selectionType, out peformance);
                    SelectedFilter = peformance.ToString();

                    if (peformance != FilterType.CUSTOM && AppStarter.IsPerformanceCalendarPopUpOpen != null && AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen)
                    {
                        AppStarter.IsPerformanceCalendarPopUpOpen.IsOpen = false;
                    }
                    switch (peformance)
                    {
                        case FilterType.DAY:
                            FromDate = DateTime.Today;
                            ToDate = DateTime.Today;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                LowFrequencyStr += localisation.LowFrequency1DayFilterStr_resx;
                            }));
                            break;
                        case FilterType.WEEK:
                            FromDate = DateTime.Today.AddDays(-7);
                            ToDate = DateTime.Today;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                LowFrequencyStr += localisation.LowFrequency1WeekFilterStr_resx;
                            }));
                            break;
                        case FilterType.MONTH:
                            FromDate = DateTime.Today.AddMonths(-1);
                            ToDate = DateTime.Today;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                LowFrequencyStr += localisation.LowFrequency1MonthFilterStr_resx;
                            }));
                            break;
                        case FilterType.YEAR:
                            FromDate = DateTime.Today.AddYears(-1);
                            ToDate = DateTime.Today;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                LowFrequencyStr += localisation.LowFrequency1YearFilterStr_resx;
                            }));
                            break;
                        case FilterType.FIVEYEAR:
                            FromDate = DateTime.Today.AddYears(-5);
                            ToDate = DateTime.Today;
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                LowFrequencyStr += localisation.LowFrequency5YearFilterStr_resx;
                            }));
                            break;
                        case FilterType.FULLVIEW:
                            FromDate = null;
                            ToDate = DateTime.Today;
                            LowFrequencyStr = localisation.LowFrequencyFullViewFilterStr_resx;
                            break;
                        case FilterType.CUSTOM:
                            GenericCustomButtonVisibility = Visibility.Collapsed;
                            CustomDateTextboxVisibility = Visibility.Visible;
                            if (!CustomStartDate.Contains("dd") && !CustomEndDate.Contains("dd"))
                            {
                                FromDate = Convert.ToDateTime(CustomStartDate);
                                ToDate = Convert.ToDateTime(CustomEndDate);
                            }

                            LowFrequencyStr += localisation.LowFrequencyFilterAppendStr_resx;
                            break;
                    }
                    //SelectedFilter = peformance.ToString();
                    if (peformance != FilterType.CUSTOM)
                    {
                        GenericCustomButtonVisibility = Visibility.Visible;
                        CustomDateTextboxVisibility = Visibility.Collapsed;
                        CustomStartDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
                        CustomEndDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
                    }
                }

                if ((PreviousFromDate != FromDate || PreviousToDate != ToDate) && SelectedFilter != null)
                    LoadTilesData();

                PreviousFromDate = FromDate;
                PreviousToDate = ToDate;
            }
            catch (Exception ex)
            {

                LogHelper.LogError(ex);
            }


        }
        public void SelctedDate()
        {
            if (!CustomStartDate.Contains("yy") && !CustomEndDate.Contains("yy") && CustomSelectedDate != null)
            {
                CustomStartDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
                CustomEndDate = localisation.PerformanceFilterDefaultDateWatermark_resx;
            }
            if (CustomSelectedDate != null)
            {
                if (!setOnce)
                {

                    CustomStartDate = CustomSelectedDate.Date.ToShortDateString();
                    DatePickerStartDate = CustomSelectedDate;
                    setOnce = true;
                    DateString = localisation.EndDateStr_resx;
                }
                else
                {
                    CustomEndDate = CustomSelectedDate.Date.ToShortDateString();
                    DatePickerStartDate = Convert.ToDateTime(localisation.PerformanceFilterDefaultStartDate_resx);
                    setOnce = false;
                    IsCalendarPopUpOpen = false;
                    DateString = localisation.StartDateStr_resx;
                }
            }
            if (CustomDateTextboxVisibility == Visibility.Visible && !CustomStartDate.Contains("yy") && !CustomEndDate.Contains("yy"))
                FilterSelection(FilterType.CUSTOM.ToString());
        }

        public void DatePickerClick()
        { 
            IsCalendarPopUpOpen = true;
            CustomDateTextboxVisibility = Visibility.Visible;
            GenericCustomButtonVisibility = Visibility.Collapsed;
            SelectedFilter = FilterType.CUSTOM.ToString(); ;
        }
        private void MouseClick()
        {
            if(IsCalendarPopUpOpen)
                IsCalendarPopUpOpen =false;
        }
        /// <summary>
        /// method for loading all tiles data.
        /// </summary>
        private void LoadTilesData()
        {
            var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());

            task.ContinueWith(async (priorTask) =>
            {
                // DOING SOME HEAVY LIFTING WORK HERE
                await Task.Run(() => GetPerformanceCount());
                await Task.Run(() => GetHighRiskData());
                await Task.Run(() => GetTreatmentBreakdown());


                // TRYING TO FORCE THE UI TO REFRESH
                Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                {

                }));
            });
            task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
        }

        /// method for count the data of performance tiles.
        private async Task GetPerformanceCount()
        {
            mainViewModel.TotalCallMethodCount++;
            var PerformanceTilesData = await PerformanceTilesData_ResultObj.GetPerformanceTilesData(fromDate, toDate);
            if (PerformanceTilesData != null)
            {
                PerformanceTilesData_ResultObj = PerformanceTilesData;
            }
            mainViewModel.CalledMethodCount++;
        }
        // method to get the data of HighRiskPatientsData Tile.
        private async Task GetHighRiskData()
        {
            var modelResult = new HighRiskPatientTileData_Result();
            try
            {
                if (mainViewModel != null)
                {
                    mainViewModel.TotalCallMethodCount++;

                    //var ExaminationHistoryData = await new ExaminationHistoryData_Result().GetExaminationHistoryTileData(SelectedPatientId, SelectedFilterDate);
                    var highRiskPatients = await modelResult.GetHighRiskPatientTileData(fromDate, toDate);
                    if (highRiskPatients != null && highRiskPatients.Count() > 1)
                    {

                    HighRiskPatientTileData_ResultList = highRiskPatients.ToObservableCollection();
                    HighRiskPatientTileData_ResultListObj = highRiskPatients.ToObservableCollection();
                    #region get Soring on load
                    var model = GetSorting();
                    SaveSorting(model, null);
                    SetSorting(model);
                    #endregion

                  
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var patientcount = HighRiskPatientTileData_ResultList.Select(x => x.PatientId).Count();
                            PerformanceTilesData_ResultObj.PatientCount = patientcount;
                            List<int> excisionCount = HighRiskPatientTileData_ResultList.Select(x => x.ExcisionCount).ToList();
                            int sumOfExcisionCount = excisionCount.Sum();
                            PerformanceTilesData_ResultObj.ExcisionCount = sumOfExcisionCount;
                        });
                    }

                    mainViewModel.CalledMethodCount++;

                    #region To do in future becoze all count will be get seprately and bind UI async with background data process

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            //mainViewModel.CalledMethodCount++;
        }
        // Method to get the data of TreatmentBreakdown tile.
        private async Task GetTreatmentBreakdown()
        {
            mainViewModel.TotalCallMethodCount++;
            ObservableCollection<TreatmentBreakdownTileModel> treatmentList = null;
            SeriesCollection = new SeriesCollection();
            SeriesCollection.Clear();
            try
            {
                var mapper = Mappers.Pie<TreatmentBreakdownTileModel>().Value(x => x.Value);
                Charting.For<TreatmentBreakdownTileModel>(mapper);

                var TreatmentBreakdownData = await new TreatmentBreakdownTileModel().GetTreatmentBreakdown(fromDate, toDate);
                if (TreatmentBreakdownData != null)
                {
                    treatmentList = TreatmentBreakdownData.ToObservableCollection();
                    PerformanceTilesData_ResultObj.TotalBodyMappingSessions = Convert.ToInt32(treatmentList[1].Value);
                    if (!treatmentList.All(x => x.Value == 0))
                    {
                        foreach (var item in treatmentList)
                        {

                            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                SeriesCollection.Add(
                                    new PieSeries
                                    {
                                        Title = item.Title,
                                        Values = new ChartValues<double> { item.Value },
                                        Fill = (Brush)new BrushConverter().ConvertFromString(item.Color),
                                        Stroke = Brushes.Transparent,
                                        StrokeThickness = 0,
                                    });
                            }));
                        }
                    }

                }
                mainViewModel.CalledMethodCount++;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        #region Sorting
        private string _sortColName;
        public string SortColName
        {
            get { return _sortColName; }
            set { _sortColName = value; OnPropertyChanged(); }
        }
        private string _sortColOrder;
        public string SortColOrder
        {
            get { return _sortColOrder; }
            set { _sortColOrder = value; OnPropertyChanged(); }
        }
        public void Sorting(DataGridColumnHeader clickedColumn)
        {
            try
            {
                DataGridSettings model = GetSorting();
                model = SaveSorting(model, clickedColumn.Column);
                SetSorting(model);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private DataGridSettings GetSorting()
        {
            return mainViewModel.DataGridSettingList.Where(x => x.UserId == 0
            && x.ModuleId == (int)ModuleNames.Performance
            && x.GridName == DataGridNames.HighRiskPatient.ToString()
            && x.GridConfigName == DataGridConfigName.ColumnSort.ToString()).FirstOrDefault();
        }

        private void SetSorting(DataGridSettings model)
        {
            if (model != null)
            {
                string[] values = model.GridConfigValue.Split(':').Select(sValue => sValue.Trim()).ToArray();
                if (values.Length > 1)
                {
                    SortColName = values[0];
                    SortColOrder = values[1];
                    HighRiskPatientTileData_ResultList = HighRiskPatientTileData_ResultListObj;
                    if (values[1].ToEnum<ListSortDirection>() == ListSortDirection.Ascending)
                    {
                        HighRiskPatientTileData_ResultList = HighRiskPatientTileData_ResultList.OrderBy(source => GetOrderSource(source, values[0])).ToObservableCollection();
                    }
                    else
                    {
                        HighRiskPatientTileData_ResultList = HighRiskPatientTileData_ResultList.OrderByDescending(source => GetOrderSource(source, values[0])).ToObservableCollection();
                    }
                }
            }
        }
        private object GetOrderSource(HighRiskPatientTileData_Result source, string name)
        {
            var propInfo = source.GetType().GetRuntimeProperty(name);

            if (propInfo != null)

                // get the current sort column value
                return propInfo.GetValue(source);

            return null;
        }

        private DataGridSettings SaveSorting(DataGridSettings model, DataGridColumn clickedColumn = null)
        {
            string prevSortColName = string.Empty;
            ListSortDirection? prevSortColValue = null;

            if (model == null)
            {
                model = new DataGridSettings();
                model = GetDataGridSettingModel(model);
                model.Id = 0;
                model.GridConfigName = DataGridConfigName.ColumnSort.ToString();
                model.GridConfigValue = "";
            }
            else
            {
                string[] values = model.GridConfigValue.Split(':').Select(sValue => sValue.Trim()).ToArray();
                prevSortColName = values[0];

                if (clickedColumn != null)
                {
                    if (prevSortColName != clickedColumn.SortMemberPath)
                    {
                        clickedColumn.SortDirection = ListSortDirection.Ascending;
                    }
                    else
                    {
                        prevSortColValue = values[1].ToEnum<ListSortDirection>();
                        clickedColumn.SortDirection = prevSortColValue == ListSortDirection.Ascending
                            ? ListSortDirection.Descending
                            : ListSortDirection.Ascending;
                    }

                    model.GridConfigValue = clickedColumn.SortMemberPath + ":" + clickedColumn.SortDirection;
                }
             
            }

            model.InsertOrUpdateDataGridSettings(model);

            var data = new DataGridSettings().GetDataGridSettings();
            if (data != null)
                mainViewModel.DataGridSettingList = data.ToObservableCollection();

            return model;
        }
        private DataGridSettings GetDataGridSettingModel(DataGridSettings dataGridSettings)
        {
            dataGridSettings.UserId = 0;
            dataGridSettings.ModuleId = (int)ModuleNames.Performance;
            dataGridSettings.GridName = DataGridNames.HighRiskPatient.ToString();
            return dataGridSettings;
        }

       
        #endregion

        #endregion

    }
}