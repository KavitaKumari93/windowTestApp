using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class TrackUploadStatusLog : PropertyChangeHelper
    {
        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }


        private int _patientId;
        public int PatientId
        {
            get { return _patientId; }
            set { _patientId = value; OnPropertyChanged(); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); }
        }

        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set { _patientName = value; OnPropertyChanged(); }
        }

        private int _imageNumber;
        public int ImageNumber
        {
            get { return _imageNumber; }
            set { _imageNumber = value; OnPropertyChanged(); }
        }



        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged(); }
        }

        private string _imageName;
        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; OnPropertyChanged(); }
        }


        private string _machineName;
        public string MachineName
        {
            get { return _machineName; }
            set
            {
                _machineName = value;
                OnPropertyChanged();
            }
        }

        private string _fileType;
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; OnPropertyChanged(); }
        }

        private string _fileStatus;
        public string FileStatus
        {
            get { return _fileStatus; }
            set { _fileStatus = value; OnPropertyChanged(); }
        }

        private string _macAddress;
        public string MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; OnPropertyChanged(); }
        }

        private DateTime _uploadTime;
        public DateTime UploadTime
        {
            get { return _uploadTime; }
            set { _uploadTime = value; OnPropertyChanged(); }
        }

        private string _uploadStatus;
        public string UploadStatus
        {
            get { return _uploadStatus; }
            set { _uploadStatus = value; OnPropertyChanged(); }
        }



        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; OnPropertyChanged(); }
        }

        private int _createdBy;
        public int CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; OnPropertyChanged(); }
        }

        private DateTime _modifiedOn;
        public DateTime ModifiedOn
        {
            get { return _modifiedOn; }
            set { _modifiedOn = value; OnPropertyChanged(); }
        }
        private int _modifiedBy;
        public int ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; OnPropertyChanged(); }
        }

        private DateTime _deletedOn;
        public DateTime DeletedOn
        {
            get { return _deletedOn; }
            set { _deletedOn = value; OnPropertyChanged(); }
        }

        private int _deletedBy;
        public int DeletedBy
        {
            get { return _deletedBy; }
            set { _deletedBy = value; OnPropertyChanged(); }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }

        private string _iconUploadStatus;
        public string IconUploadStatus
        {
            get { return _iconUploadStatus; }
            set { _iconUploadStatus = value; OnPropertyChanged(); }
        }

        private string _videoName;
        public string VideoName
        {
            get { return _videoName; }
            set { _videoName = value; OnPropertyChanged(); }
        }

        private string _videoUploadStatus;
        public string VideoUploadStatus
        {
            get { return _videoUploadStatus; }
            set { _videoUploadStatus = value; OnPropertyChanged(); }
        }

        private string _imageUploadStatus;
        public string ImageUploadStatus
        {
            get { return _imageUploadStatus; }
            set { _imageUploadStatus = value; OnPropertyChanged(); }
        }

        private string _iconName;
        public string IconName
        {
            get { return _iconName; }
            set { _iconName = value; OnPropertyChanged(); }
        }


        #endregion

        #region Methods
        public async Task<IEnumerable<TrackUploadStatusLog>> GetTrackUploadStatusLogList(int patientId)
        {
            try
            {
                using (var repo = new GenericRepository<TrackUploadStatusLog>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetTrackUploadStatusLogs, new { @patientId = patientId });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

    }
    #endregion
}
