using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class PerformanceTilesData_Result : PropertyChangeHelper
    {
        public PerformanceTilesData_Result()
        {

        }

        #region Property

        private int lowFrequentPatientCount;
        public int LowFrequentPatientCount
        {
            get { return lowFrequentPatientCount; }
            set { lowFrequentPatientCount = value; OnPropertyChanged(); }
        }

        private string doctorName;
        public string DoctorName
        {
            get { return doctorName; }
            set { doctorName = value; OnPropertyChanged(); }
        }

        private int totalBodyMappingSessions;
        public int TotalBodyMappingSessions
        {
            get { return totalBodyMappingSessions; }
            set { totalBodyMappingSessions = value; OnPropertyChanged(); }
        }
        private int opinionRequestCount;
        public int OpinionRequestCount
        {
            get { return opinionRequestCount; }
            set { opinionRequestCount = value; OnPropertyChanged(); }
        }

        private int patientCount;
        public int PatientCount
        {
            get { return patientCount; }
            set { patientCount = value; OnPropertyChanged(); }
        }

        private int excisionCount;
        public int ExcisionCount
        {
            get { return excisionCount; }
            set { excisionCount = value; OnPropertyChanged(); }
        }


        #endregion

        public async Task<PerformanceTilesData_Result> GetPerformanceTilesData(DateTime? fromDate, DateTime toDate)
        {
            try
            {
                using (var repo = new GenericRepository<PerformanceTilesData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetPerformanceTilesDataByDateRange, new { @fromDate = fromDate, @toDate = toDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }



    }
}
