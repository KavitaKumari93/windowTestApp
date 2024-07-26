using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using Localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class AboutViewModel : PropertyChangeHelper
    {
        #region Properties 

        // Collection of items related to the About section
        private ObservableCollection<AboutItem> aboutItemsList;
        public ObservableCollection<AboutItem> AboutItemsList
        {
            get { return aboutItemsList; }
            set { aboutItemsList = value; OnPropertyChanged(); }
        }

        // Collection of medical device data
        private ObservableCollection<MedicalDevicesData_Result> medicalDevicesData_ResultObj;
        public ObservableCollection<MedicalDevicesData_Result> MedicalDevicesData_ResultObj
        {
            get { return medicalDevicesData_ResultObj; }
            set { medicalDevicesData_ResultObj = value; OnPropertyChanged(); }
        }

        // Version number of the application
        private string versionNumber;
        public string VersionNumber
        {
            get { return versionNumber; }
            set { versionNumber = value; OnPropertyChanged(); }
        }

        #endregion

        // Command to navigate back
        public ICommand BackCommand { get; private set; }

        // Constructor
        public AboutViewModel()
        {
            // Initialize back command
            BackCommand = new RelayCommandHelper(x => OnBack(x));
            // Populate About section items
            FillAboutItems();
            // Retrieve fotofinder medical devices data
            GetDevicesListData();
            // Retrieve database version data
            GetDatabaseVersionData();
        }

        #region Methods

        // Method to handle back navigation
        private void OnBack(object letfBottomMenu)
        {
            try
            {
                // Navigate back to the previous view
                MainViewModel vm = (MainViewModel)AppStarter.mainVM;
                vm.BindSelectedView();
            }
            catch (Exception ex)
            {
                // Log any errors that occur
                LogHelper.LogError(ex);
            }
        }

        /// <summary>
        /// 
        /// Method to populate About section items
        /// </summary>
        public void FillAboutItems()
        {
            // Initialize the AboutItemsList collection
            AboutItemsList = new ObservableCollection<AboutItem>();

            // Add items to the About section
            AboutItemsList.Add(new AboutItem()
            {
                Icon = "CEMark",
                Text = Localisation.AboutProductManufacturing_resx
            });

            // Add more About section items...
        }

        // Method to retrieve fotofinder medical devices data asynchronously
        private async void GetDevicesListData()
        {
            try
            {
                // Retrieve medical devices data
                var medicalDevices = await new MedicalDevicesData_Result().GetMedicalDevicesData();

                // If data is retrieved successfully, update the collection
                if (medicalDevices != null)
                {
                    MedicalDevicesData_ResultObj = medicalDevices.ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur
                LogHelper.LogError(ex);
            }
        }

        // Method to retrieve database version data asynchronously
        public async void GetDatabaseVersionData()
        {
            try
            {
                // Access the database repository
                using (var repo = new GenericRepository<ServiceTilesData_Result>())
                {
                    // Execute stored procedure to get version data
                    var details = await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetServiceTileVersionData, null);

                    // Update the version number
                    VersionNumber = details.VersionNumber;
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur
                LogHelper.LogError(ex);
            }
        }

        #endregion
    }

    public class AboutItem
    {
        public string Icon { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
    }
}



