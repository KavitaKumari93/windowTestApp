using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using localisation=FF.Cockpit.Common.Properties.Resources;
namespace FF.Cockpit.UI.ViewModels
{
    public class OperationalViewModel : SchedulerViewModel
    {

        public OperationalViewModel()
        {
            AppointmentEditorOpenCommand = new RelayCommandHelper<AppointmentEditorOpeningEventArgs>(OpenAppointmentEditor);
            AppointmentDroppingCommand = new RelayCommandHelper<AppointmentDroppingEventArgs>(DropAppointment);
            AppointmentResizingCommand = new RelayCommandHelper<AppointmentResizingEventArgs>(ResizeAppointment);
            NavigateToPatientDashBoardCommand = new RelayCommandHelper(x => NavigateToPatientDashboard(x));
            WeakEventManager<SchedulerFilterEvent, EventArgs>.AddHandler(
                                                            source: SchedulerFilterEvent.SchedulerFilterEventInstance,
                                                            eventName: nameof(SchedulerFilterEvent.SchedulerFilterEventHandler),
                                                            LoadOperationalData);

            LoadOperationalData(null, null);
        }

        #region Properties

        private ObservableCollection<AppointedPatient> _appointedPatientList;
        public ObservableCollection<AppointedPatient> AppointedPatientList
        {
            get { return _appointedPatientList; }
            set { _appointedPatientList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AppointedDoctor> _appointedDoctorList;
        public ObservableCollection<AppointedDoctor> AppointedDoctorList
        {
            get { return _appointedDoctorList; }
            set { _appointedDoctorList = value; OnPropertyChanged(); }
        }


        private SeriesCollection _followUpSeriesCollection;
        public SeriesCollection FollowUpSeriesCollection
        {
            get { return _followUpSeriesCollection; }
            set { _followUpSeriesCollection = value; OnPropertyChanged(); }
        }

        private SeriesCollection _treatmentBreakDownSeriesCollection;
        public SeriesCollection TreatmentBreakDownSeriesCollection
        {
            get { return _treatmentBreakDownSeriesCollection; }
            set { _treatmentBreakDownSeriesCollection = value; OnPropertyChanged(); }
        }
        private SeriesCollection roomOccupancySeries;
        public SeriesCollection RoomOccupancySeries
        {
            get { return roomOccupancySeries; }
            set { roomOccupancySeries = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> roomOccupancyXAxisLabels;
        public ObservableCollection<string> RoomOccupancyXAxisLabels
        {
            get { return roomOccupancyXAxisLabels; }
            set { roomOccupancyXAxisLabels = value; OnPropertyChanged(); }
        }
        private ObservableCollection<string> roomOccupancyYAxisLabels;
        public ObservableCollection<string> RoomOccupancyYAxisLabels
        {
            get { return roomOccupancyYAxisLabels; }
            set { roomOccupancyYAxisLabels = value; OnPropertyChanged(); }
        }

        private string _totalScheduledTime;
        public string TotalScheduledTime
        {
            get { return _totalScheduledTime; }
            set { _totalScheduledTime = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods

        public void LoadOperationalData(object sender, EventArgs args)
        {
            var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());

            task.ContinueWith(async (priorTask) =>
            {
                // DOING SOME HEAVY LIFTING WORK HERE
                await Task.Run(() => GetFollowUpTileData());
                await Task.Run(() => GetAppointedPatientsList());
                await Task.Run(() => GetDoctorsAppointmentsList());
                await Task.Run(() => GetTreatmentBreakdownTileData());
                await Task.Run(() => GetScheduledHoursDetails());
                await Task.Run(() => GetRoomOccupancyTileData());
            });
        }

        private async void GetAppointedPatientsList()
        {
            try
            {
                var appointedPatients = await new AppointedPatient().GetAppointedPatientsTilesData(SelectedFromDate, SelectedToDate);
                if (appointedPatients != null)
                {
                    AppointedPatientList = appointedPatients.ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private async void GetFollowUpTileData()
        {
            ObservableCollection<OperationalFollowTileModel> followData = null;
            FollowUpSeriesCollection = new SeriesCollection();

            try
            {
                var mapper = Mappers.Pie<OperationalFollowTileModel>().Value(x => x.Value);
                Charting.For<OperationalFollowTileModel>(mapper);

                var followTileModelData = await new OperationalFollowTileModel().GetFollowUpData(SelectedFromDate, SelectedToDate);
                if (followTileModelData != null)
                {
                    followData = followTileModelData.ToObservableCollection();

                    if (!followData.All(x => x.Value == 0))
                    {
                        foreach (var item in followData)
                        {
                            App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                var data = new PieSeries
                                {
                                    Title = item.Title,
                                    Values = new ChartValues<double> { item.Value },
                                    Fill = (Brush)new BrushConverter().ConvertFromString(item.Color),
                                    Stroke = Brushes.Transparent,
                                    StrokeThickness = 0,
                                };

                                if (FollowUpSeriesCollection.Where(x => x.Title == data.Title).Count() == 0)
                                    FollowUpSeriesCollection.Add(data);
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private async void GetDoctorsAppointmentsList()
        {
            try
            {
                var doctorAppointment = await new AppointedDoctor().GetDoctorsAppointmentsListData(SelectedFromDate, SelectedToDate);
                if (doctorAppointment != null)
                {
                    AppointedDoctorList = doctorAppointment.ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private async void GetTreatmentBreakdownTileData()
        {

            ObservableCollection<OperationalTreatmentBreakDownTileModel> treatmentData = null;
            TreatmentBreakDownSeriesCollection = new SeriesCollection();
            try
            {
                var mapper = Mappers.Pie<OperationalTreatmentBreakDownTileModel>().Value(x => x.Value);
                Charting.For<OperationalTreatmentBreakDownTileModel>(mapper);

                var treatmentBreakDownModelData = await new OperationalTreatmentBreakDownTileModel().GetOperationalTreatmentBreakDownData(SelectedFromDate, SelectedToDate);
                if (treatmentBreakDownModelData != null)
                {
                    treatmentData = treatmentBreakDownModelData.ToObservableCollection();

                    if (!treatmentData.All(x => x.Value == 0))
                    {
                        foreach (var item in treatmentData)
                        {
                            App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                var data = new PieSeries
                                {
                                    Title = item.Title,
                                    Values = new ChartValues<double> { item.Value },
                                    Fill = (Brush)new BrushConverter().ConvertFromString(item.Color),
                                    Stroke = Brushes.Transparent,
                                    StrokeThickness = 0,
                                };

                                if (TreatmentBreakDownSeriesCollection.Where(x => x.Title == data.Title).Count() == 0)
                                    TreatmentBreakDownSeriesCollection.Add(data);
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        private async void GetScheduledHoursDetails()
        {
            try
            {
                var totalScheduledTime = await new OperationalScheduledHoursTileData_Result().GetOperationalScheduledHoursTileData(SelectedFromDate, SelectedToDate);
                if (totalScheduledTime != null)
                    TotalScheduledTime = totalScheduledTime.TotalTime;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        
        public RelayCommandHelper NavigateToPatientDashBoardCommand { get; set; }
        private void NavigateToPatientDashboard(object pateintID)
        {
            try
            {
                if (mainViewModel != null && pateintID != null)
                {
                    //Navigate to Patient Dashboard
                    AppStarter.IsNavigateToPatientDashboard = true;
                    AppStarter.IsNavigateFromOperationalDashboard= true;
                    App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        mainViewModel.SelectedMenu = mainViewModel.NavigationModelList[2];
                        mainViewModel.CurrentView = new DashboardViewModel();
                    }));
                    var dashboardViewModel = (DashboardViewModel)mainViewModel.CurrentView;
                    dashboardViewModel.PatientSelectedChanged((int)pateintID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #endregion


        private void ResizeAppointment(AppointmentResizingEventArgs e)
        {
            e.CanCommit = false;
        }
        private void DropAppointment(AppointmentDroppingEventArgs e)
        {
            e.Cancel = true;
        }
        private new void OpenAppointmentEditor(AppointmentEditorOpeningEventArgs e)
        {
            if (mainViewModel != null)
            {
                //Navigate to Scheduler
                AppStarter.IsNavigateToPatientDashboard = true;
                AppStarter.IsNavigateFromOperationalDashboard = true;
                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    mainViewModel.SelectedMenu = mainViewModel.NavigationModelList[1];
                    mainViewModel.CurrentView = new SchedulerViewModel();
                }));
                base.OpenAppointmentEditor(e);
            }
            e.Cancel = true;
        }
        private async Task GetRoomOccupancyTileData()
        {
            var roomsInformation = await new RoomOccupancyTileDataResult().GetRoomOccupancyTileData(SelectedFromDate, SelectedToDate);
            if (roomsInformation.Count() > 0)
                SetChart(roomsInformation);
        }

        public ChartValues<ObservableValue> RoomOcupancyList(IEnumerable<RoomOccupancyTileDataResult> occupancyTileDataResult, string roomType)
        {
            var values = new ChartValues<ObservableValue>();
            foreach (var rooms in occupancyTileDataResult)
            {
                if (roomType == localisation.OccupancyChartlbl1_resx)
                    values.Add(new ObservableValue(rooms.Occupancy));
                else
                    values.Add(new ObservableValue(rooms.Available));
            }
            return values;
        }
        public void SetChart(IEnumerable<RoomOccupancyTileDataResult> occupancyTileDataResult)
        {

            if (occupancyTileDataResult != null)
            {
                RoomOccupancySeries = new SeriesCollection();
                RoomOccupancyXAxisLabels = new ObservableCollection<string>();

                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                {
                    occupancyTileDataResult.ToObservableCollection();
                    RoomOccupancySeries = new SeriesCollection
                    {
                          new StackedColumnSeries
                          {
                              Title = localisation.OccupancyChartlbl1_resx,
                              Values = RoomOcupancyList(occupancyTileDataResult,localisation.OccupancyChartlbl1_resx),
                              MaxColumnWidth=30,
                              ColumnPadding=15,
                              Fill = new SolidColorBrush(Color.FromRgb(132, 107, 81))// First color
                          },
                          new StackedColumnSeries
                          {
                              Title = localisation.OccupancyChartlbl2_resx,
                              Values =   RoomOcupancyList(occupancyTileDataResult,localisation.OccupancyChartlbl2_resx),
                              StackMode = StackMode.Values,
                              Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255)), // Second color
                              MaxColumnWidth=30,
                              ColumnPadding=15,
                              LabelsPosition=BarLabelPosition.Top,
                          }
                    };
                    RoomOccupancyXAxisLabels.AddRange(occupancyTileDataResult.Select(items => items.RoomName));
                }));
            }
        }
    }

}

