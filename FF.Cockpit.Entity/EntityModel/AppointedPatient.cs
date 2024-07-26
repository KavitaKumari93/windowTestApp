using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class AppointedPatient : PropertyChangeHelper
    {
        public AppointedPatient()
        {
        }

        #region Properties
        private int _patientId;
        public int PatientId
        {
            get { return _patientId; }
            set { _patientId = value; OnPropertyChanged(); }
        }
        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set { _patientName = value; OnPropertyChanged(); }
        }
        private string _patientRecordNumber;
        public string PatientRecordNumber
        {
            get { return _patientRecordNumber; }
            set { _patientRecordNumber = value; OnPropertyChanged(); }
        }

        private string _doctorName;
        public string DoctorName
        {
            get { return _doctorName; }
            set { _doctorName = value; OnPropertyChanged(); }
        }

        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set { _roomId = value; OnPropertyChanged(); }
        }
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; OnPropertyChanged(); }
        }

        private string _appointmentType;
        public string AppointmentType
        {
            get { return _appointmentType; }
            set { _appointmentType = value; OnPropertyChanged(); }
        }
        private string _riskcategory;
        public string Riskcategory
        {
            get { return _riskcategory; }
            set { _riskcategory = value; OnPropertyChanged(); }
        }
        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods
        public async Task<IEnumerable<AppointedPatient>> GetAppointedPatientsTilesData(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var repo = new GenericRepository<AppointedPatient>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetAppointedPatients, new { @startDate = startDate, @endDate = endDate });
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
