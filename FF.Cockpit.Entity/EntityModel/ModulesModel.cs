using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class ModulesModel : PropertyChangeHelper
    {
        #region Properties


        private int _moduleId;
        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; OnPropertyChanged(); }
        }
        private string _moduleName;
        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; OnPropertyChanged(); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods
        public IEnumerable<ModulesModel> GetModuleNames()
        {
            try
            {
                using (var repo = new GenericRepository<ModulesModel>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetModules, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public int InsertOrUpdateModuleName(ModulesModel modulesModel)
        {
            try
            {
                using (var repo = new GenericRepository<ModulesModel>())
                {

                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateModule,
                          new
                          {
                              @id = modulesModel.ModuleId,
                              @moduleName = modulesModel.ModuleName
                          });
                    return model.ModuleId;
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
