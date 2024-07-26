using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class RoomOccupancyTileDataResult
    {
        #region Property
        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set { _roomId = value; }
        }
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; }
        }
        private double _occupancy;
        public double Occupancy
        {
            get { return _occupancy; }
            set { _occupancy = value; }
        }
        private double _available;
        public double Available
        {
            get { return _available; }
            set { _available = value; }
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<RoomOccupancyTileDataResult>> GetRoomOccupancyTileData(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var repo = new GenericRepository<RoomOccupancyTileDataResult>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetRoomOccupancyReportByDateRange, new { @FromDate = startDate, @ToDate = endDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
            return Enumerable.Empty<RoomOccupancyTileDataResult>();
        }
        #endregion
    }
}
