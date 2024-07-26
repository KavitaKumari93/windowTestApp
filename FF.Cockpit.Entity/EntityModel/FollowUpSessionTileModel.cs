using FF.Cockpit.Common;
using FF.Cockpit.Entity.StoreProcedure_Result;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class FollowUpSessionTileModel : PropertyChangeHelper
    {
        #region Property

        private int? _patientId;
        private DateTime? _selectedFilter;

        private ObservableCollection<SessionYearsData_Result> sessionYearsResultList;
        public ObservableCollection<SessionYearsData_Result> SessionYearsResultList
        {
            get { return sessionYearsResultList; }
            set { sessionYearsResultList = value; OnPropertyChanged(); }
        }

        private SessionYearsData_Result selectedSessionYear;
        public SessionYearsData_Result SelectedSessionYear
        {
            get { return selectedSessionYear; }
            set { selectedSessionYear = value; OnPropertyChanged(); }
        }

        private ChartValues<FollowUpSessionTileData_Result> sessionPoint;
        public ChartValues<FollowUpSessionTileData_Result> SessionPoint
        {
            get { return sessionPoint; }
            set { sessionPoint = value; OnPropertyChanged(); }
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

        #endregion

        #region Constructor
        public FollowUpSessionTileModel()
        {
            sessionYearsResultList = new ObservableCollection<SessionYearsData_Result>();
        }
        #endregion

        #region Method
        public async Task<FollowUpSessionTileModel> GetFolloUpSessionTileData(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                _patientId = patientId;
                _selectedFilter = selectedFilterDate;

                sessionYearsResultList.Clear();
                var data = await Task.Run(() => new SessionYearsData_Result().GetSessionYearsData(_patientId, _selectedFilter));

                if (data != null)
                {
                    sessionYearsResultList = data.ToObservableCollection();
                    selectedSessionYear = sessionYearsResultList.FirstOrDefault();
                    await SetChartData();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return this;

        }


        public async Task SetChartData()
        {
            SessionPoint = new ChartValues<FollowUpSessionTileData_Result>();
            SessionPoint.Clear();
            XLabels = new List<string>();
            YLabels = new List<int>();

            var data = await Task.Run(() => new FollowUpSessionTileData_Result().GetFollowUpSessionById(_patientId, _selectedFilter));

            if (data != null)
            {
                var result = data.OrderBy(x => x.SessionDate).ToObservableCollection();

                if (result.Count > 0)
                {
                    SessionPoint.AddRange(result);
                    XLabels.AddRange(result.Select(x => x.SessionDateString));
                }

                if (SessionPoint.Count > 0)
                {
                    var maxY = SessionPoint.Select(x => Math.Abs(x.SessionDuration)).Max();
                    if (maxY <= 120)
                    {
                        Max_YAxis = 120;
                        Min_YAxis = 0;
                        Gap_YAxis = 15;
                    }
                    else if (maxY > 120 && maxY <= 180)
                    {
                        Max_YAxis = 180;
                        Min_YAxis = 0;
                        Gap_YAxis = 15;
                    }
                    else if (maxY > 180 && maxY <= 360)
                    {
                        Max_YAxis = 360;
                        Min_YAxis = 0;
                        Gap_YAxis = 30;
                    }
                    else
                    {
                        Max_YAxis = maxY;
                        Min_YAxis = 0;
                        Gap_YAxis = 45;
                    }
                }
            }
        }
        #endregion
    }
}
