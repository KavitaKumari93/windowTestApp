using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public  class ServiceTilesData_Result:PropertyChangeHelper
    {
        #region Properties 
        private string versionNumber;
        public string VersionNumber
        {
            get { return versionNumber; }
            set { versionNumber = value; OnPropertyChanged(); }
        }
        private string versionUpdatedNumber;
        public string VersionUpdatedNumber
        {
            get { return versionUpdatedNumber; }
            set { versionUpdatedNumber = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods 
        public async Task<ServiceTilesData_Result> GetServiceTilesVersionData()
        {
            try
            {
                using (var repo = new GenericRepository<ServiceTilesData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetServiceTileVersionData, null);
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
