using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class MedicalDevicesData_Result : PropertyChangeHelper
    {
        #region Properties 
        private string manufacturer;
        public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; OnPropertyChanged(); }
        }
        private string serialNumber;
        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; OnPropertyChanged(); }
        }
        private Int64 currentCounter;
        public Int64 CurrentCounter
        {
            get { return currentCounter; }
            set { currentCounter = value; OnPropertyChanged(); }
        }
        private string deviceModel;
        public string DeviceModel
        {
            get { return deviceModel; }
            set { deviceModel = value; OnPropertyChanged(); }
        }
        private DateTime firstUsage;
        public DateTime FirstUsage
        {
            get { return firstUsage; }
            set { firstUsage = value; OnPropertyChanged(); }
        }
        private DateTime lastMaintenance;
        public DateTime LastMaintenance
        {
            get { return lastMaintenance; }
            set { lastMaintenance = value; OnPropertyChanged(); }
        }
        private Int64 lastMaintenanceCounter;
        public Int64 LastMaintenanceCounter
        {
            get { return lastMaintenanceCounter; }
            set { lastMaintenanceCounter = value; OnPropertyChanged(); }
        }
        private Int64 maxImageReleases;
        public Int64 MaxImageReleases
        {
            get { return maxImageReleases; }
            set { maxImageReleases = value; OnPropertyChanged(); }
        }
        private DateTime lastUsage;
        public DateTime LastUsage
        {
            get { return lastUsage; }
            set { lastUsage = value; OnPropertyChanged(); }
        }
        private Int64 maxServiceIntervalDays;
        public Int64 MaxServiceIntervalDays
        {
            get { return maxServiceIntervalDays; }
            set { maxServiceIntervalDays = value; OnPropertyChanged(); }
        }
       
        private Int64 remainingReleasesToMaintenance;
        public Int64 RemainingReleasesToMaintenance
        {

            get { return SetRemainingReleasesToMaintenance(); }
            set { remainingReleasesToMaintenance = value; OnPropertyChanged(); }
        }
        private TimeSpan maxTimePerServiceInterval;
        public TimeSpan MaxTimePerServiceInterval
        {

            get { return maxTimePerServiceInterval; }
            set { maxTimePerServiceInterval = new TimeSpan( 0, 0, 0, 0); ; OnPropertyChanged(); }
        }
       
        #endregion

        #region Methods 
        public async Task<IEnumerable<MedicalDevicesData_Result>> GetMedicalDevicesData()
        {
            try
            {
                using (var repo = new GenericRepository<MedicalDevicesData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetFotoFinderDevicesInformation, null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public Int64 SetRemainingReleasesToMaintenance()
        {
            Int64 nextMaintenance = 0;
            if (LastMaintenanceCounter==0)
            {
                nextMaintenance = MaxServiceIntervalDays;
            }
            else
            {
                nextMaintenance = LastMaintenanceCounter + MaxServiceIntervalDays;
            }
            if (nextMaintenance > CurrentCounter)
            {
                nextMaintenance = nextMaintenance - CurrentCounter;
            }
            return nextMaintenance;
        }
        

       
        #endregion 
    }
}
