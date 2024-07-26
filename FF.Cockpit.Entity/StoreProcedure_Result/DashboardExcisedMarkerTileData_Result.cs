using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class DashboardExcisedMarkerTileData_Result : PropertyChangeHelper
    {
        public DashboardExcisedMarkerTileData_Result()
        {

        }
        #region Property
        private int markText;
        public int MarkText
        {
            get { return markText; }
            set { markText = value; OnPropertyChanged(); }
        }
        private DateTime dateofexcision;
        public DateTime DateofExcision
        {
            get { return dateofexcision; }
            set { dateofexcision = value; OnPropertyChanged(); }
        }
        #endregion
        public IEnumerable<DashboardExcisedMarkerTileData_Result> GetExcisedMarkerList(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<DashboardExcisedMarkerTileData_Result>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetExcisedMarker, new { @patientObjectID = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
    }
}