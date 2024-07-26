using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class OperationalTreatmentBreakDownTileModel:BasePieChartModel
    {
        #region Method

        public async Task<IEnumerable<OperationalTreatmentBreakDownTileModel>> GetOperationalTreatmentBreakDownData(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (var repo = new GenericRepository<OperationalTreatmentBreakDownTileModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetOperationalTreatmentBreakdown, new { @FromDate = fromDate, @ToDate = toDate });
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
