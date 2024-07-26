using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class UpcomingAppointmentModel : PropertyChangeHelper
    {
        #region Properties
        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }
        private int _appointmentId;
        public int AppointmentId
        {
            get { return _appointmentId; }
            set { _appointmentId = value; OnPropertyChanged(); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

        private double _numberOfConfiguredDays;
        public double NumberOfConfiguredDays
        {
            get { return _numberOfConfiguredDays; }
            set
            {
                _numberOfConfiguredDays = value;
                _downloadDateTime = StartTime.AddDays(value);
                OnPropertyChanged();
            }
        }

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

        private string _doctorName;
        public string DoctorName
        {
            get { return _doctorName; }
            set { _doctorName = value; OnPropertyChanged(); }
        }

        private DateTime _patientDOB;
        public DateTime PatientDOB
        {
            get { return _patientDOB; }
            set { _patientDOB = value; OnPropertyChanged(); }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
        }

        private int _localImagesCount;
        public int LocalImagesCount
        {
            get { return _localImagesCount; }
            set { _localImagesCount = value; OnPropertyChanged(); }
        }

        private int _awsImagesCount;
        public int AwsImagesCount
        {
            get { return _awsImagesCount; }
            set { _awsImagesCount = value; OnPropertyChanged(); }
        }

        private DateTime _downloadDateTime;
        public DateTime DownloadDateTime
        {
            get { return _downloadDateTime; }
            set { _downloadDateTime = value; OnPropertyChanged(); }
        }

        private DateTime _uploadDateTime;
        public DateTime UploadDateTime
        {
            get { return _uploadDateTime; }
            set { _uploadDateTime = value; OnPropertyChanged(); }
        }

        private string _syncType;
        public string SyncType
        {
            get { return _syncType; }
            set { _syncType = value; OnPropertyChanged(); }
        }
        private string _syncDownloadStatus;
        public string SyncDownloadStatus
        {
            get { return _syncDownloadStatus; }
            set { _syncDownloadStatus = value; OnPropertyChanged(); }
        }
        private string _syncUploadStatus;
        public string SyncUploadStatus
        {
            get { return _syncUploadStatus; }
            set { _syncUploadStatus = value; OnPropertyChanged(); }
        }
        private bool _isLocalCleared;
        public bool IsLocalCleared
        {
            get { return _isLocalCleared; }
            set { _isLocalCleared = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<UpcomingAppointmentModel>> GetUpcomingAppointmentList(DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                using (var repo = new GenericRepository<UpcomingAppointmentModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetUpcomingAppointments, new { @StartDate = FromDate, @EndDate = ToDate });
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
