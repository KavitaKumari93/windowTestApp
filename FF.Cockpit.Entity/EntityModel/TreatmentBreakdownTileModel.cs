using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class TreatmentBreakdownTileModel :BasePieChartModel
    {

        #region Constructor
        public TreatmentBreakdownTileModel()
        {

        }
        #endregion

        #region Method

        public async Task<IEnumerable<TreatmentBreakdownTileModel>> GetTreatmentBreakdown(DateTime? fromDate, DateTime toDate)
        {
            try
            {
                using (var repo = new GenericRepository<TreatmentBreakdownTileModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetTreatmentBreakdownDataByDateRange, new { @fromDate = fromDate, @toDate = toDate });
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
