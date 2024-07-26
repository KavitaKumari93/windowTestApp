using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class AppointedDoctor : PropertyChangeHelper
    {
        public AppointedDoctor()
        {

        }

        #region Property
        private int _doctorId;
        public int DoctorId
        {
            get { return _doctorId; }
            set { _doctorId = value; OnPropertyChanged(); }
        }

        private string _doctorName;
        public string DoctorName
        {
            get { return _doctorName; }
            set { _doctorName = value; OnPropertyChanged(); }
        }

        private int _totalAppointments;
        public int TotalAppointments
        {
            get { return _totalAppointments; }
            set { _totalAppointments = value; OnPropertyChanged(); }
        }

        private string _appointments;
        public string Appointments
        {
            get { return _appointments; }
            set { _appointments = value; OnPropertyChanged(); }
        }

        private string _totalDuration;
        public string TotalDuration
        {
            get { return _totalDuration; }
            set { _totalDuration = value; OnPropertyChanged(); }
        }


        #endregion

        public async Task<IEnumerable<AppointedDoctor>> GetDoctorsAppointmentsListData(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                using (var repo = new GenericRepository<AppointedDoctor>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetAppointedDoctors, new { @StartDate = StartDate, @EndDate = EndDate });
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