using FF.Cockpit.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class MiscellaneousSettingsModel : PropertyChangeHelper
    {
        #region Properties

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
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

        public IEnumerable<MiscellaneousSettingsModel> GetMiscellaneousData_sync()
        {
            try
            {
                using (var repo = new GenericRepository<MiscellaneousSettingsModel>())
                {
                  return  repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetMiscellaneousData, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public void InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel miscellaneous)
        {
            try
            {
                using (var repo = new GenericRepository<MiscellaneousSettingsModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateMiscellaneousData,
                          new
                          {
                              @id = miscellaneous.Id,
                              @name = miscellaneous.Name.ToString(),
                              @value = miscellaneous.Value,
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
