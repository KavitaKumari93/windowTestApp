using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class FollowUpSessionTileData_Result
    {
        #region Property

        private string sessionName;
        public string SessionName
        {
            get { return sessionName; }
            set { sessionName = value; }
        }

        private DateTime sessionDate;
        public DateTime SessionDate
        {
            get { return sessionDate; }
            set { if (sessionDate != null) { sessionDateString = value.ToString(ConstantHelper.DateFormat); sessionDate = value; } }
        }

        private int sessionDuration;
        public int SessionDuration
        {
            get { return sessionDuration; }
            set { sessionDuration = value; }
        }

        private int microImageCount;
        public int MicroImageCount
        {
            get { return microImageCount; }
            set { microImageCount = value; }
        }

        private int tbmImageCount;
        public int TBMImageCount
        {
            get { return tbmImageCount; }
            set { tbmImageCount = value; }
        }

        private string sessionDateString;
        public string SessionDateString
        {
            get { return sessionDateString; }
            set { sessionDateString = value; }
        }
        #endregion

        #region Constructor
        public FollowUpSessionTileData_Result()
        {
        }
        #endregion

        #region Methods

        public async Task<IEnumerable<FollowUpSessionTileData_Result>> GetFollowUpSessionById(int? patientId, DateTime? selectedFilter)
        {
            try
            {
                using (var repo = new GenericRepository<FollowUpSessionTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetFollowUpSessionTileDataByPatientId, new { @PatientId = patientId, @selectedFilterDate = selectedFilter });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        #endregion
    }
}
