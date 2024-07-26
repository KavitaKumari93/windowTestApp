using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class OperationalFollowTileModel: BasePieChartModel
    {
        #region Constructor
        public OperationalFollowTileModel()
        {

        }
        #endregion

        #region Method

        public async Task<IEnumerable<OperationalFollowTileModel>> GetFollowUpData(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var repo = new GenericRepository<OperationalFollowTileModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetOperationalFollowUpData, new { @startDate = startDate, @endDate = endDate });
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
