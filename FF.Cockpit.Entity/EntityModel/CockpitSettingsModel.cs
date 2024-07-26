using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class CockpitSettingsModel : PropertyChangeHelper
    {
        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private DateTime _lastSyncedOn;
        public DateTime LastSyncedOn
        {
            get { return _lastSyncedOn; }
            set { _lastSyncedOn = value; OnPropertyChanged(); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; OnPropertyChanged(); }
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

        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; OnPropertyChanged(); }
        }
        #endregion

        #region Method
        public CockpitSettingsModel GetCockpitSettingInfo()
        {
            try
            {
                using (var repo = new GenericRepository<CockpitSettingsModel>())
                {
                    return repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_GetCockpitSettings, new {});
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public int InsertOrUpdateCockpitSettingInfo(DateTime? lastSyncedDate)
        {
            try
            {
                using (var repo = new GenericRepository<CockpitSettingsModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateCockpitSettings,
                          new
                          {
                              @LastSyncedOn = lastSyncedDate
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
        #endregion
    }
}
