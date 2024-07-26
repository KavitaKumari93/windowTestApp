using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class ImagesStatusBreakDownModel : BasePieChartModel
    {

        #region Constructor
        public ImagesStatusBreakDownModel()
        {

        }
        #endregion

        #region Method

        public async Task<IEnumerable<ImagesStatusBreakDownModel>> GetPatientImagesStatusFollowUp(int patientId)
        {
            try
            {
                using (var repo = new GenericRepository<ImagesStatusBreakDownModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetPatientImagesStatusFollowUp, new { @patientId = patientId });
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
