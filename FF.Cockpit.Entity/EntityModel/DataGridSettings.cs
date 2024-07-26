using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{

    public class DataGridSettings : PropertyChangeHelper
    {
        public DataGridSettings()
        {
        }

        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }
        private int _moduleId;
        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; OnPropertyChanged(); }
        }

        private string _gridName;
        public string GridName
        {
            get { return _gridName; }
            set { _gridName = value; OnPropertyChanged(); }
        }

        private string _gridConfigName;
        public string GridConfigName
        {
            get { return _gridConfigName; }
            set { _gridConfigName = value; OnPropertyChanged(); }
        }

        private string _gridConfigValue;
        public string GridConfigValue
        {
            get { return _gridConfigValue; }
            set { _gridConfigValue = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods
        public IEnumerable<DataGridSettings> GetDataGridSettings()
        {
            try
            {
                using (var repo = new GenericRepository<DataGridSettings>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetDataGridSettings, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public void InsertOrUpdateDataGridSettings(DataGridSettings dataGridSetting)
        {
            try
            {
                using (var repo = new GenericRepository<DataGridSettings>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateGridSetting,
                          new
                          {
                              @id = dataGridSetting.Id,
                              @userId = dataGridSetting.UserId,
                              @moduleId = dataGridSetting.ModuleId,
                              @gridName = dataGridSetting.GridName,
                              @gridConfigName = dataGridSetting.GridConfigName,
                              @gridConfigValue = dataGridSetting.GridConfigValue
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
