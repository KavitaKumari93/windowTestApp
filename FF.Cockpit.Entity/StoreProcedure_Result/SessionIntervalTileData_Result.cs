using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class SessionIntervalTileData_Result : PropertyChangeHelper
    {
        public SessionIntervalTileData_Result()
        {
        }

        #region Property
        private string frequencyLevel;
        public string FrequencyLevel
        {
            get { return frequencyLevel; }
            set { frequencyLevel = value; OnPropertyChanged(); }
        }

        private DateTime appointmentDate;
        public DateTime AppointmentDate
        {
            get { return appointmentDate; }
            set { appointmentDate = value; OnPropertyChanged(); }
        }

        private string interval;
        public string Interval
        {
            get { return interval; }
            set { interval = value; OnPropertyChanged(); }
        }
        private string _nextAppointmentDate;
        public string NextAppointmentDate
        {
            get { return _nextAppointmentDate; }
            set { _nextAppointmentDate = value; OnPropertyChanged(); }
        }

        private DateTime _appointmentStartAt;
        public DateTime AppointmentStartAt
        {
            get { return _appointmentStartAt; }
            set { _appointmentStartAt = value; OnPropertyChanged(); }
        }
        private DateTime _appointmentEndAt;
        public DateTime AppointmentEndAt
        {
            get { return _appointmentEndAt; }
            set { _appointmentEndAt = value; OnPropertyChanged(); }
        }
        #endregion

        public async Task<SessionIntervalTileData_Result> GetSessionIntervalTileData(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<SessionIntervalTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetSessionIntervalTileDataByPatientId, new { @patientId = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public async Task<int> UpdateSessionIntervalTileData(int? patientId, DateTime appointmentDate)
        {
            try
            {
                using (var repo = new GenericRepository<int>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_UpdateSessionIntervalTileData, new { @patientId = patientId, @appointmentDate = appointmentDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }
    }
}
