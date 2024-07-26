using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class MiscellaneousAWSConfigurationModel : PropertyChangeHelper
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        private string awsBucketName;
        public string AWSBucketName
        {
            get { return awsBucketName; }
            set { awsBucketName = value; OnPropertyChanged(); }
        }
        private string awsSecretKey;
        public string AWSSecretKey
        {
            get { return awsSecretKey; }
            set { awsSecretKey = value; OnPropertyChanged(); }
        }
        private string awsAccessKey;
        public string AWSAccessKey
        {
            get { return awsAccessKey; }
            set { awsAccessKey = value; OnPropertyChanged(); }
        }
        private string aWSBucketRegion;
        public string AWSBucketRegion
        {
            get { return aWSBucketRegion; }
            set { aWSBucketRegion = value; OnPropertyChanged(); }
        }
        private string locationURL;
        public string LocationURL
        {
            get { return locationURL; }
            set { locationURL = value; OnPropertyChanged(); }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }

        private string aWSConfigSubmitText;
        public string AWSConfigSubmitText
        {
            get { return aWSConfigSubmitText; }
            set { aWSConfigSubmitText = value; OnPropertyChanged(); }
        }
        #region MethodS

        public IEnumerable<MiscellaneousAWSConfigurationModel> GetMiscellaneousConfigurationDetails()
        {
            try
            {
                using (var repo = new GenericRepository<MiscellaneousAWSConfigurationModel>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetMiscellaneousAWSConfigurationDetails, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public int InsertMiscellaneousConfigurationData(MiscellaneousAWSConfigurationModel configurationDetailModel)
        {
            try
            {
                using (var repo = new GenericRepository<MiscellaneousAWSConfigurationModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateMiscellaneousAWSConfigurationDetails,
                          new
                          {
                              @id = configurationDetailModel.Id,
                              @bucket = configurationDetailModel.AWSBucketName,
                              @access = configurationDetailModel.AWSAccessKey,
                              @secret = configurationDetailModel.AWSSecretKey,
                              @region = configurationDetailModel.AWSBucketRegion,
                              @url = configurationDetailModel.LocationURL,
                              @isActive = true
                          });
                    return (model.Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }

        #endregion
    }
}
