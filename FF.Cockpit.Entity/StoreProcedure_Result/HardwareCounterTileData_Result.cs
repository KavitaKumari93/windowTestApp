using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class HardwareCounterTileData_Result : PropertyChangeHelper
    {
        public HardwareCounterTileData_Result()
        {
        }

        #region Property

        private int rowNumber;
        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; OnPropertyChanged(); }
        }

        private string manufacturer;
        public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; OnPropertyChanged(); }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set { body = value; OnPropertyChanged(); }
        }

        private int counter;
        public int Counter
        {
            get { return counter; }
            set { counter = value; OnPropertyChanged(); }
        }

        private int nextService;
        public int NextService
        {
            get { return nextService; }
            set { nextService = value; OnPropertyChanged(); }
        }

        private string lastServiceDate;
        public string LastServiceDate
        {
            get { return lastServiceDate; }
            set { lastServiceDate = value; OnPropertyChanged(); }
        }

        private string nextServiceDate;
        public string NextServiceDate
        {
            get { return nextServiceDate; }
            set { nextServiceDate = value; OnPropertyChanged(); }
        }

        private decimal status;
        public decimal Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }


        #endregion

        public async Task<IEnumerable<HardwareCounterTileData_Result>> GetHardwareCounterTileData()
        {
            try
            {
                using (var repo = new GenericRepository<HardwareCounterTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetHardwareCounterTileData, new { });
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
