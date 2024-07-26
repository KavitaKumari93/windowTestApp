using FF.Cockpit.Common;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FF.Cockpit.UI.ViewModels
{
    public class ServiceViewModel : PropertyChangeHelper
    {
        #region Properties 
        MainViewModel mainViewModel { get; set; }

        private ObservableCollection<HardwareCounterTileData_Result> hardwareCounterTileData_ResultList;
        public ObservableCollection<HardwareCounterTileData_Result> HardwareCounterTileData_ResultList
        {
            get { return hardwareCounterTileData_ResultList; }
            set { hardwareCounterTileData_ResultList = value; OnPropertyChanged(); }
        }

        private ServiceTilesData_Result serviceTilesData_ResultObj;
        public ServiceTilesData_Result ServiceTilesData_ResultObj
        {
            get { return serviceTilesData_ResultObj; }
            set { serviceTilesData_ResultObj = value; OnPropertyChanged(); }
        }
        #endregion

        public ServiceViewModel()
        {
            try
            {
                mainViewModel = (MainViewModel)AppStarter.mainVM;
                LoadTilesData();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void LoadTilesData()
        {
            var task = Task.Factory.StartNew(() => mainViewModel.LoaderShow());

            task.ContinueWith(async (priorTask) =>
            {
                await Task.Run(() => GetVersionData());
                await Task.Run(() => GetHardwareCounterData());
            });
            task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
        }

        private async void GetHardwareCounterData()
        {
            mainViewModel.TotalCallMethodCount++;
            var HardwareCounterTileData = await new HardwareCounterTileData_Result().GetHardwareCounterTileData();
            if (HardwareCounterTileData != null)
            {
                HardwareCounterTileData_ResultList = HardwareCounterTileData.ToObservableCollection();
            }
            mainViewModel.CalledMethodCount++;
        }
        private async void GetVersionData()
        {
            mainViewModel.TotalCallMethodCount++;
            var versionTileData = await new ServiceTilesData_Result().GetServiceTilesVersionData();
            if (versionTileData != null)
            {
                ServiceTilesData_ResultObj = versionTileData;
                AppStarter.DatabaseVersion = versionTileData.VersionNumber;
                string[] version = versionTileData.VersionNumber.Split('.');
                Int32 update = Convert.ToInt32(version[2]) + 1;
                ServiceTilesData_ResultObj.VersionUpdatedNumber = version[0]+'.'+ version[1]+ '.'+ update.ToString();
            }
            mainViewModel.CalledMethodCount++;
        }
    }
}
