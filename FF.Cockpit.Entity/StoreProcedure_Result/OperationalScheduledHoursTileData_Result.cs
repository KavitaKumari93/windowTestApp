using FF.Cockpit.Common;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class OperationalScheduledHoursTileData_Result : PropertyChangeHelper
    {
        #region Properties
        private string _totalTime;
        public string TotalTime
        {
            get { return _totalTime; }
            set { _totalTime = value; }
        }
        #endregion

        #region Methods
        public async Task<OperationalScheduledHoursTileData_Result> GetOperationalScheduledHoursTileData(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var repo = new GenericRepository<OperationalScheduledHoursTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetScheduledAppointmentTimeByDateRange, new { @FromDate = startDate, @ToDate = endDate });
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
