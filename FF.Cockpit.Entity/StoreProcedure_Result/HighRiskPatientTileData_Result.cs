using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class HighRiskPatientTileData_Result : PropertyChangeHelper
    {
        public HighRiskPatientTileData_Result()
        {
        }
        #region Property

        private int patientId;
        public int PatientId
        {
            get { return patientId; }
            set { patientId = value; OnPropertyChanged(); }
        }

        private string patientName;
        public string PatientName
        {
            get { return patientName; }
            set { patientName = value; OnPropertyChanged(); }
        }

        //private string dOB;
        //public string DOB
        //{
        //    get { return dOB; }
        //    set { dOB = value; OnPropertyChanged(); }
        //}

        private double bodyScanCount;
        public double BodyScanCount
        {
            get { return bodyScanCount; }
            set { bodyScanCount = value; OnPropertyChanged(); }
        }

        private int markerCount;
        public int MarkerCount
        {
            get { return markerCount; }
            set { markerCount = value; OnPropertyChanged(); }
        }

        private int microImageCount;
        public int MicroImageCount
        {
            get { return microImageCount; }
            set { microImageCount = value; OnPropertyChanged(); }
        }

        private int excisionCount;
        public int ExcisionCount
        {
            get { return excisionCount; }
            set { excisionCount = value; OnPropertyChanged(); }
        }

        //private string comment;
        //public string Comment
        //{
        //    get { return comment; }
        //    set { comment = value; OnPropertyChanged(); }
        //}
        private int _sessionsCount;
        public int SessionsCount
        {
            get { return _sessionsCount; }
            set { _sessionsCount = value; OnPropertyChanged(); }
        }
        
        private int _avgSessionDuration;
        public int AvgSessionDuration
        {
            get { return _avgSessionDuration; }
            set { _avgSessionDuration = value; OnPropertyChanged(); }
        }

        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; OnPropertyChanged(); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<HighRiskPatientTileData_Result>> GetHighRiskPatientTileData(DateTime? fromDate, DateTime toDate)
        {
            try
            {
                using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHighRiskPatientTileDataByDateRange, new { @fromDate = fromDate, @toDate = toDate});
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public int GetHighRiskPatientTileCount(DateTime? fromDate, DateTime toDate)
        {
            try
            {
                using (var repo = new GenericRepository<int>())
                {
                    return  repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_GetHighRiskPatientTileDataCount, new { @FromDate = fromDate, @toDate = toDate});
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }
        public  HighRiskPatientTileData_Result SyncHighRiskPatientDumpData()
        {
            try
            {
                using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
                { 
                return repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_MergeHighRiskPatientTileData, null,true);
                }
            }
            catch (Exception  ex)
            {
                LogHelper.LogError(ex);
                   return null;
            }
        }

        //public async Task<IEnumerable<HighRiskPatientTileData_Result>> GetHighRiskPatientBodyScanCount(int patientId, DateTime? fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
        //        {
        //            return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHighRiskPatientBodyScanCount, new { @patientId = patientId, @fromDate = fromDate, @toDate = toDate });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError(ex);
        //        return null;
        //    }
        //}
        //public async Task<IEnumerable<HighRiskPatientTileData_Result>> GetHighRiskPatientMarkerCount(int patientId, DateTime? fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
        //        {
        //            return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHighRiskPatientMarkerCount, new { @patientId = patientId, @fromDate = fromDate, @toDate = toDate });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError(ex);
        //        return null;
        //    }
        //}
        //public async Task<IEnumerable<HighRiskPatientTileData_Result>> GetHighRiskPatientMicroImagesCount(DateTime? fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
        //        {
        //            return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHighRiskPatientMicroImageCount, new { @fromDate = fromDate, @toDate = toDate });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError(ex);
        //        return null;
        //    }
        //}
        //public async Task<IEnumerable<HighRiskPatientTileData_Result>> GetHighRiskPatientExcisionCount(DateTime? fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        using (var repo = new GenericRepository<HighRiskPatientTileData_Result>())
        //        {
        //            return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHighRiskPatientExcisionCount, new { @fromDate = fromDate, @toDate = toDate });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError(ex);
        //        return null;
        //    }
        //}
        #endregion
    }
}
