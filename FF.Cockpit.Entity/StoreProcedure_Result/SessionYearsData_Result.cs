using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class SessionYearsData_Result : PropertyChangeHelper
    {
        public SessionYearsData_Result()
        {

        }

        #region Property
        private int sessionYear;
        public int SessionYear
        {
            get { return sessionYear; }
            set { sessionYear = value; OnPropertyChanged(); }
        }
        private string sessionYearName;
        public string SessionYearName
        {
            get { return sessionYearName; }
            set { sessionYearName = value; OnPropertyChanged(); }
        }
        #endregion

        public async Task<IEnumerable<SessionYearsData_Result>> GetSessionYearsData(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<SessionYearsData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetSessionYearsDataByPatientId, new { @PatientId = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex) { LogHelper.LogError(ex); return null; }
        }
    }
}
