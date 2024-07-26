using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class TrendsTilesData_Result
    {
        private readonly GenericRepository<TrendsTilesData_Result> _repository;
        #region Property

        private string year;
        public string Year
        {
            get { return year; }
            set { year = value; }
        }
        private int quarter;
        public int Quarter
        {
            get { return quarter; }
            set { quarter = value; }
        }
        private string month;
        public string Month
        {
            get { return month; }
            set { month = value; }
        }
        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int bodyMappingSessionCount;
        public int BodyMappingSessionCount
        {
            get { return bodyMappingSessionCount; }
            set { bodyMappingSessionCount = value; }
        }
        private int dermoscopeSessionCount;
        public int DermoscopeSessionCount
        {
            get { return dermoscopeSessionCount; }
            set { dermoscopeSessionCount = value; }
        }

        #endregion
        public TrendsTilesData_Result()
        {
            _repository = new GenericRepository<TrendsTilesData_Result>();
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate, string storedProcedure)
        {
            try
            {
                switch (trendsYear)
                {
                    case TrendsYears.OneYear:
                        return await _repository.ExcuteProcedureWithMultiResult(storedProcedure + "ByOneYear", new { @fromDate = fromDate, @toDate = toDate });
                    case TrendsYears.FiveYear:
                        return await _repository.ExcuteProcedureWithMultiResult(storedProcedure + "ByFiveYear", new { @fromDate = fromDate, @toDate = toDate });
                    case TrendsYears.TwentyYear:
                        return await _repository.ExcuteProcedureWithMultiResult(storedProcedure + "ByTwentyYear", new { @fromDate = fromDate, @toDate = toDate });
                    default:
                        throw new ArgumentException("Invalid TrendsYear specified.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetExaminationTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            return await GetTrendsData(trendsYear, fromDate, toDate, "GetExaminationTrends");
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetMicroImagesTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            return await GetTrendsData(trendsYear, fromDate, toDate, "GetMicroImagesTrends");
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetSessionDurationTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            return await GetTrendsData(trendsYear, fromDate, toDate, "GetSessionDurationTrends");
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetPatientTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            return await GetTrendsData(trendsYear, fromDate, toDate, "GetPatientTrends");
        }

        public async Task<IEnumerable<TrendsTilesData_Result>> GetAvgSessionDurationTrendsData(TrendsYears trendsYear, DateTime? fromDate, DateTime? toDate)
        {
            return await GetTrendsData(trendsYear, fromDate, toDate, "GetAvgSessionDurationTrends");
        }
    }
}
