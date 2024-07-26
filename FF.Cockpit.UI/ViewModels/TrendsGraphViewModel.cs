using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows;
using System.Windows.Threading;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace FF.Cockpit.UI.ViewModels
{
    public class TrendsGraphViewModel : PropertyChangeHelper
    {
        public MainViewModel mainViewModel { get; set; }

        public TrendsGraphViewModel()
        {

            FilterSelectionCommand = new RelayCommandHelper(x => FilterSelection(x));

            SelectedFilter = FilterType.FIVEYEAR.ToString();
            FilterSelection(SelectedFilter);
            mainViewModel = (MainViewModel)AppStarter.mainVM;
        }
        #region Property

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; OnPropertyChanged(); }
        }
        private DateTime _toDate;
        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; OnPropertyChanged(); }
        }
        private Visibility _isOneYearSelected;
        public Visibility IsOneYearSelected
        {
            get { return _isOneYearSelected; }
            set { _isOneYearSelected = value; OnPropertyChanged(); }
        }
        private Visibility _isFiveYearSelected;
        public Visibility IsFiveYearSelected
        {
            get { return _isFiveYearSelected; }
            set { _isFiveYearSelected = value; OnPropertyChanged(); }
        }
        private Visibility _isTwentyYearSelected;
        public Visibility IsTwentyYearSelected
        {
            get { return _isTwentyYearSelected; }
            set { _isTwentyYearSelected = value; OnPropertyChanged(); }
        }

        private string selectedFilter;
        public string SelectedFilter
        {
            get { return selectedFilter; }
            set { selectedFilter = value; OnPropertyChanged(); }
        }

        private TrendsModel examinationTrendsModel;
        public TrendsModel ExaminationTrendsModel
        {
            get { return examinationTrendsModel; }
            set { examinationTrendsModel = value; OnPropertyChanged(); }
        }
        private TrendsModel trendsModelDetails;
        public TrendsModel TrendsModelDetails
        {
            get { return trendsModelDetails; }
            set { trendsModelDetails = value; OnPropertyChanged(); }
        }
        private TrendsModel patientTrendsModel;
        public TrendsModel PatientTrendsModel
        {
            get { return patientTrendsModel; }
            set { patientTrendsModel = value; OnPropertyChanged(); }
        }
        private TrendsModel sessionDurationTrendsModel;
        public TrendsModel SessionDurationTrendsModel
        {
            get { return sessionDurationTrendsModel; }
            set { sessionDurationTrendsModel = value; OnPropertyChanged(); }
        }
        private TrendsModel microImagesTrendsModel;
        public TrendsModel MicroImagesTrendsModel
        {
            get { return microImagesTrendsModel; }
            set { microImagesTrendsModel = value; OnPropertyChanged(); }
        }
        private TrendsModel avgSessionDurationTrendsModel;
        public TrendsModel AvgSessionDurationTrendsModel
        {
            get { return avgSessionDurationTrendsModel; }
            set { avgSessionDurationTrendsModel = value; OnPropertyChanged(); }
        }

        #endregion

        #region Commands
        public RelayCommandHelper FilterSelectionCommand { get; private set; }
        #endregion

        #region Quarter
        private void SetLastQuarterDates(out DateTime startDate, out DateTime endDate, string quarter)
        {
            int currentYear = DateTime.Now.Year;
            int startMonth = 0;
            int endMonth = 0;
            int endDay = 0;

            switch (quarter)
            {
                case "Q1":
                    startMonth = 1;
                    endMonth = 3;
                    endDay = 31;
                    break;

                case "Q2":
                    startMonth = 4;
                    endMonth = 6;
                    endDay = 30;
                    break;

                case "Q3":
                    startMonth = 7;
                    endMonth = 9;
                    endDay = 30;
                    break;

                case "Q4":
                    startMonth = 10;
                    currentYear--;
                    endMonth = 12;
                    endDay = 31;
                    break;
            }

            startDate = new DateTime(currentYear, startMonth, 1);
            endDate = new DateTime(currentYear, endMonth, endDay);
        }

        private string GetCurrentQuarterDates(int month)
        {
            string quarter = string.Empty;

            if (month <= 3)
            {
                quarter = "Q1";
            }
            else if (month > 3 && month <= 6)
            {
                quarter = "Q2";
            }
            else if (month > 6 && month <= 9)
            {
                quarter = "Q3";
            }
            else if (month > 9 && month <= 12)
            {
                quarter = "Q4";
            }
            return quarter;
        }
        #endregion

        #region Methods
        private void LoadTrendsUC()
        {
            var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());

            task.ContinueWith(async (priorTask) =>
           {
               await Task.Run(() => GetTrendChartsData(TrendsYears.FiveYear, FromDate, ToDate));

           });

            task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
        }
        private async void FilterSelection(object selectionType)
        {
            try
            {
                if (selectionType == null)
                    return;

                if (Enum.TryParse((string)selectionType, out FilterType filterType))
                {
                    TrendsYears trendsYears = TrendsYears.FiveYear;
                    SelectedFilter = filterType.ToString();

                    switch (filterType)
                    {
                        case FilterType.YEAR:
                            SetDateRangeForOneYear();
                            trendsYears = TrendsYears.OneYear;
                            break;
                        case FilterType.FIVEYEAR:
                            SetDateRangeForFiveYears();
                            trendsYears = TrendsYears.FiveYear;
                            break;
                        case FilterType.TWENTYYEAR:
                            SetDateRangeForTwentyYears();
                            trendsYears = TrendsYears.TwentyYear;
                            break;
                    }
                    await GetTrendChartsData(trendsYears, FromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void SetDateRangeForOneYear()
        {
            IsOneYearSelected = Visibility.Visible;
            IsFiveYearSelected = Visibility.Collapsed;
            IsTwentyYearSelected = Visibility.Collapsed;

            DateTime now = DateTime.Now;
            FromDate = new DateTime(now.Year-1, now.Month+1, 1);
            ToDate = now;
        }

        private void SetDateRangeForFiveYears()
        {
            IsOneYearSelected = Visibility.Collapsed;
            IsFiveYearSelected = Visibility.Visible;
            IsTwentyYearSelected = Visibility.Collapsed;

            DateTime now = DateTime.Now;
            string currentQuarter = GetCurrentQuarterDates(now.Month);
            DateTime lastQuarterEndDate;
            SetLastQuarterDates(out _, out lastQuarterEndDate, currentQuarter);

            FromDate = lastQuarterEndDate.AddYears(-5).AddDays(1);
            ToDate = lastQuarterEndDate;
        }

        private void SetDateRangeForTwentyYears()
        {
            IsOneYearSelected = Visibility.Collapsed;
            IsFiveYearSelected = Visibility.Collapsed;
            IsTwentyYearSelected = Visibility.Visible;

            int currentYear = DateTime.Now.Year;
            FromDate = new DateTime(currentYear - 19, 1, 1);
            ToDate = new DateTime(currentYear, 12, 31);
        }
        public async Task GetTrendChartsData(TrendsYears trendsYears, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var tasks = new List<Task<TrendsModel>>();

                tasks.Add(new TrendsModel().GeTrendsData(TrendsType.ExaminationTrends, trendsYears, fromDate, toDate));
                tasks.Add(new TrendsModel().GeTrendsData(TrendsType.PatientTrends, trendsYears, fromDate, toDate));
                tasks.Add(new TrendsModel().GeTrendsData(TrendsType.SessionDurationTrends, trendsYears, fromDate, toDate));
                tasks.Add(new TrendsModel().GeTrendsData(TrendsType.AvgSessionDurationTrends, trendsYears, fromDate, toDate));
                tasks.Add(new TrendsModel().GeTrendsData(TrendsType.MicroImagesTrends, trendsYears, fromDate, toDate));
                await Task.WhenAll(tasks);

                ExaminationTrendsModel = tasks[0].Result;
                PatientTrendsModel = tasks[1].Result;
                SessionDurationTrendsModel = tasks[2].Result;
                AvgSessionDurationTrendsModel = tasks[3].Result;
                MicroImagesTrendsModel = tasks[4].Result;

                var trendsDateRange = fromDate.ToString("yyyy") + " - " + toDate.ToString("yyyy");

                foreach (var model in new[] { ExaminationTrendsModel, PatientTrendsModel, SessionDurationTrendsModel, AvgSessionDurationTrendsModel, MicroImagesTrendsModel })
                {
                    if (model.TrendsYear == TrendsYears.OneYear)
                        model.TrendsDateRange = fromDate.ToString("yyyy/MM", CultureInfo.InvariantCulture) + " - " + toDate.ToString("yyyy/MM", CultureInfo.InvariantCulture);
                    else
                        model.TrendsDateRange = trendsDateRange;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        //private async void FilterSelection(object selectionType)
        //{
        //    try
        //    {

        //        if (selectionType != null)
        //        {
        //            TrendsYears trendsYears = TrendsYears.FiveYear;
        //            Enum.TryParse<FilterType>((string)selectionType, out FilterType filterType);
        //            SelectedFilter = filterType.ToString();
        //            switch (filterType)
        //            {

        //                case FilterType.YEAR:
        //                    IsOneYearSelected = Visibility.Visible; IsFiveYearSelected = Visibility.Collapsed; IsTwentyYearSelected = Visibility.Collapsed;
        //                    DateTime dateTime = DateTime.Now;
        //                    DateTime from_OneYear = dateTime.AddMonths(-11);
        //                    from_OneYear = new DateTime(from_OneYear.Year, from_OneYear.Month, 1);
        //                    DateTime to_OneYear = dateTime;
        //                    FromDate = from_OneYear;
        //                    ToDate = to_OneYear;
        //                    trendsYears = TrendsYears.OneYear;

        //                    break;
        //                case FilterType.FIVEYEAR:
        //                    IsOneYearSelected = Visibility.Collapsed; IsFiveYearSelected = Visibility.Visible; IsTwentyYearSelected = Visibility.Collapsed;
        //                    // from and to date for five year (exclude current Quarter)
        //                    DateTime from_FiveYear = default(DateTime);
        //                    DateTime to_FiveYear = default(DateTime);
        //                    string currentQuarter = GetCurrentQuarterDates(DateTime.Now.Month);
        //                    SetLastQuarterDates(out from_FiveYear, out to_FiveYear, currentQuarter);
        //                    FromDate = to_FiveYear.AddYears(-5).AddDays(1);
        //                    ToDate = to_FiveYear;
        //                    trendsYears = TrendsYears.FiveYear;
        //                    break;
        //                case FilterType.TWENTYYEAR:
        //                    IsOneYearSelected = Visibility.Collapsed; IsFiveYearSelected = Visibility.Collapsed; IsTwentyYearSelected = Visibility.Visible;
        //                    // from and to date for twenty year (exclude current year)
        //                    var currentYear = DateTime.Now.Year;
        //                    DateTime from_TwentyYear = DateTime.ParseExact(currentYear - 19 + "-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //                    DateTime to_TwentyYear = DateTime.ParseExact(currentYear + "-12-31", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //                    FromDate = from_TwentyYear;
        //                    ToDate = to_TwentyYear;
        //                    trendsYears = TrendsYears.TwentyYear;
        //                    break;
        //            }
        //            await GetTrendChartsData(trendsYears, FromDate, ToDate);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError(ex);
        //    }

        //}

        #endregion


    }
}
