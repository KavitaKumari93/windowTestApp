using FF.Cockpit.Common;
using LiveCharts.Defaults;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FF.Cockpit.Entity.StoreProcedure_Result;

namespace FF.Cockpit.Entity.EntityModel
{
    public class TrendsModel : PropertyChangeHelper
    {
        #region Property
        private TrendsYears trendsYear;
        public TrendsYears TrendsYear
        {
            get { return trendsYear; }
            set { trendsYear = value; OnPropertyChanged(); }
        }
        private DateTime fromDate;
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged(); }
        }
        private DateTime toDate;
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; OnPropertyChanged(); }
        }
        private string trendsDateRange;
        public string TrendsDateRange
        {
            get { return trendsDateRange; }
            set { trendsDateRange = value; OnPropertyChanged(); }
        }

        private ChartValues<TrendsTilesData_Result> sessionPoint;
        public ChartValues<TrendsTilesData_Result> SessionPoint
        {
            get { return sessionPoint; }
            set { sessionPoint = value; OnPropertyChanged(); }
        }
        private ChartValues<ObservableValue> values1;
        public ChartValues<ObservableValue> Values1
        {
            get { return values1; }
            set { values1 = value; OnPropertyChanged(); }
        }
        private ChartValues<ObservableValue> values2;
        public ChartValues<ObservableValue> Values2
        {
            get { return values2; }
            set { values2 = value; OnPropertyChanged(); }
        }
        private List<string> xLabels;
        public List<string> XLabels
        {
            get { return xLabels; }
            set { xLabels = value; OnPropertyChanged(); }
        }

        private List<int> yLabels;
        public List<int> YLabels
        {
            get { return yLabels; }
            set { yLabels = value; OnPropertyChanged(); }
        }

        private double max_YAxis;
        public double Max_YAxis
        {
            get { return max_YAxis; }
            set { max_YAxis = value; OnPropertyChanged(); }
        }

        private double min_YAxis;
        public double Min_YAxis
        {
            get { return min_YAxis; }
            set { min_YAxis = value; OnPropertyChanged(); }
        }

        private double gap_YAxis;
        public double Gap_YAxis
        {
            get { return gap_YAxis; }
            set { gap_YAxis = value; OnPropertyChanged(); }
        }

        private IEnumerable<TrendsTilesData_Result> trendsTilesData_ResultModelObj;
        public IEnumerable<TrendsTilesData_Result> TrendsTilesData_ResultModelObj
        {
            get { return trendsTilesData_ResultModelObj; }
            set { trendsTilesData_ResultModelObj = value; OnPropertyChanged(); }
        }
        #endregion

        #region Method
        public async Task<TrendsModel> GeTrendsData(TrendsType trendsType, TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                await SetDurationTrends(trendsType, trendsYear, fromDate, toDate);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return this;
        }
        public async Task SetDurationTrends(TrendsType trends, TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            SessionPoint = new ChartValues<TrendsTilesData_Result>();
            SessionPoint.Clear();
            Values1 = new ChartValues<ObservableValue>();
            values1.Clear();

            XLabels = new List<string>();
            YLabels = new List<int>();

            switch(trends)
            {
                case TrendsType.AvgSessionDurationTrends:
                    TrendsTilesData_ResultModelObj = await Task.Run(() => new TrendsTilesData_Result().GetAvgSessionDurationTrendsData(trendsYear, fromDate, toDate));
                    break;
                case TrendsType.SessionDurationTrends:
                    TrendsTilesData_ResultModelObj = await Task.Run(() => new TrendsTilesData_Result().GetSessionDurationTrendsData(trendsYear, fromDate, toDate));
                    break;
                case TrendsType.PatientTrends:
                    TrendsTilesData_ResultModelObj = await Task.Run(() => new TrendsTilesData_Result().GetPatientTrendsData(trendsYear, fromDate, toDate));
                    break;
                case TrendsType.MicroImagesTrends:
                    TrendsTilesData_ResultModelObj = await Task.Run(() => new TrendsTilesData_Result().GetMicroImagesTrendsData(trendsYear, fromDate, toDate));
                    break;
                case TrendsType.ExaminationTrends:
                    TrendsTilesData_ResultModelObj = await Task.Run(() => new TrendsTilesData_Result().GetExaminationTrendsData(trendsYear, fromDate, toDate));
                    break;

            }

            if (TrendsTilesData_ResultModelObj != null)
            {

                SessionPoint.AddRange(TrendsTilesData_ResultModelObj);
                if (trendsYear == TrendsYears.OneYear)
                    XLabels.AddRange(TrendsTilesData_ResultModelObj.Select(x => x.Month));
                else if (trendsYear == TrendsYears.FiveYear)
                {
                    TrendsTilesData_ResultModelObj.Select(c => { c.Year = "Q" + c.Quarter + "-" + c.Year; return c; }).ToList();
                    XLabels.AddRange(TrendsTilesData_ResultModelObj.Select(x => x.Year));
                }
                else
                    XLabels.AddRange(TrendsTilesData_ResultModelObj.Select(x => x.Year));

                if (SessionPoint.Count > 0)
                {
                    int maxY = 0;
                    int maxValue = 0;
                    if (trends == TrendsType.ExaminationTrends)
                    {
                        int bodyMappingMax = SessionPoint.Select(x => Math.Abs(x.BodyMappingSessionCount)).Max();
                        int dermoscopeMax = SessionPoint.Select(x => Math.Abs(x.DermoscopeSessionCount)).Max();
                        maxY = bodyMappingMax > dermoscopeMax ? bodyMappingMax : dermoscopeMax;
                    }
                    else
                    {
                        maxY = SessionPoint.Select(x => Math.Abs(x.Count)).Max();
                    }
                    maxY = maxY == 0 ? 5 : maxY;

                    if (maxY < 50)
                    {
                        long n = 10;
                        while (n * 10 < maxY) n *= 10;
                        maxValue = (int)((maxY + n - 1) / n * n);
                    }
                    else
                        maxValue = (int)(Math.Ceiling((double)maxY / 100) * 100);

                    Gap_YAxis = maxY >= 0 && maxY <= 5 ? 1 :
                               maxY >= 5 && maxY <= 20 ? 2 :
                               maxY > 10 && maxY <= 50 ? 5 :
                               maxY > 50 && maxY <= 100 ? 10 :
                               maxY > 100 && maxY <= 200 ? 20 :
                               maxY > 200 && maxY <= 500 ? 50 :
                               maxY > 200 && maxY <= 600 ? 100 :
                               maxY > 600 && maxY <= 1000 ? 200 :
                               maxY > 1000 && maxY <= 1500 ? 300 :
                               maxY > 1500 && maxY <= 3000 ? 500 :
                               maxY > 3000 && maxY <= 6000 ? 1000 :
                               maxY > 6000 && maxY <= 10000 ? 2000 :
                               maxY > 10000 && maxY <= 20000 ? 2500 :
                               maxY > 20000 && maxY <= 30000 ? 3000 :
                               maxY > 30000 && maxY <= 40000 ? 4000 :
                               maxY > 40000 && maxY <= 50000 ? 5000 :
                               maxY > 50000 && maxY <= 60000 ? 6000 :
                               maxY > 60000 && maxY <= 70000 ? 7000 :
                               maxY > 70000 && maxY <= 80000 ? 8000 :
                               maxY > 80000 && maxY <= 90000 ? 9000 :
                               maxY > 90000 && maxY <= 100000 ? 10000 :
                               maxY > 100000 && maxY <= 120000 ? 12000 :
                               maxValue;
                    Max_YAxis = Gap_YAxis <= 1 ? 5 : maxValue;
                    Min_YAxis = 0;
                }
                if (trends != TrendsType.ExaminationTrends)
                {
                    Values1 = new ChartValues<ObservableValue>();
                    foreach (var rowData in TrendsTilesData_ResultModelObj)
                    {
                        Values1.Add(new ObservableValue(rowData.Count));
                    }
                }
                else
                {
                    Values1 = new ChartValues<ObservableValue>();
                    Values2 = new ChartValues<ObservableValue>();
                    foreach (var rowData in TrendsTilesData_ResultModelObj)
                    {
                        Values1.Add(new ObservableValue(rowData.DermoscopeSessionCount));
                        Values2.Add(new ObservableValue(rowData.BodyMappingSessionCount));
                    }
                }
            }

            
        }
    }
    #endregion
}

