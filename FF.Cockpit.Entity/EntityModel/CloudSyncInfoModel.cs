using FF.Cockpit.Common;
using System;
using System.Windows.Input;

namespace FF.Cockpit.Entity.EntityModel
{
    public class CloudSyncInfoModel : PropertyChangeHelper
    {
        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private int _appointmentId;
        public int AppointmentId
        {
            get { return _appointmentId; }
            set { _appointmentId = value; OnPropertyChanged(); }
        }

        private int _patientId;
        public int PatientId
        {
            get { return _patientId; }
            set { _patientId = value; OnPropertyChanged(); }
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
        private int _syncUploadStatus;
        public int SyncUploadStatus
        {
            get { return _syncUploadStatus; }
            set { _syncUploadStatus = value; OnPropertyChanged(); }
        }
        private int _syncDownloadStatus;
        public int SyncDownloadStatus
        {
            get { return _syncDownloadStatus; }
            set { _syncDownloadStatus = value; OnPropertyChanged(); }
        }
        private bool _isLocalCleared;
        public bool IsLocalCleared
        {
            get { return _isLocalCleared; }
            set { _isLocalCleared = value; OnPropertyChanged(); }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }
        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; OnPropertyChanged(); }
        }
        private DateTime _updatedDate;
        public DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; OnPropertyChanged(); }
        }
        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; OnPropertyChanged(); }
        }
        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Method

        public int InsertOrUpdateCloudSyncInfo_sync(CloudSyncInfoModel cloudSyncInfoModel)
        {
            try
            {
                using (var repo = new GenericRepository<CloudSyncInfoModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateCloudSyncInfo,
                          new
                          {
                              @id = cloudSyncInfoModel.Id,
                              @appointmentId = cloudSyncInfoModel.AppointmentId,
                              @patientId = cloudSyncInfoModel.PatientId,
                              @downloadDateTime = cloudSyncInfoModel.DownloadDateTime,
                              @uploadDateTime = cloudSyncInfoModel.UploadDateTime,
                              @syncType = cloudSyncInfoModel.SyncType,
                              @syncDownloadStatus = cloudSyncInfoModel.SyncDownloadStatus,
                              @syncUploadStatus = cloudSyncInfoModel.SyncUploadStatus,
                              @isLocalCleared = cloudSyncInfoModel.IsLocalCleared
                          });
                    return Convert.ToInt32(model.Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }


        public void UpdateCloudSyncDownloadStatus(int appointmentId, int patientId, int syncDownloadStatus)
        {
            try
            {
                using (var repo = new GenericRepository<CloudSyncInfoModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_UpdateDownloadStatus,
                          new
                          {
                              @appointmentId = appointmentId,
                              @patientId = patientId,
                              @syncDownloadStatus = syncDownloadStatus,
                          });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        #endregion
    }
}