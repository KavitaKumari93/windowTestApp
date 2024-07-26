using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using FF.Cockpit.Entity.EntityModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System.Windows;
using System.Windows.Navigation;
using localisation = FF.Cockpit.Common.Properties.Resources;
public class MiscellaneousModel : PropertyChangeHelper
{
    
    #region Properties



   

    private IEnumerable<MiscellaneousAWSConfigurationModel> _miscellaneousConfigurations;
    public IEnumerable<MiscellaneousAWSConfigurationModel> MiscellaneousConfigurations
    {
        get { return _miscellaneousConfigurations; }
        set { _miscellaneousConfigurations = value; OnPropertyChanged(); }
    }
    private IEnumerable<MiscellaneousSettingsModel> _miscellaneousSettingsModel;
    public IEnumerable<MiscellaneousSettingsModel> MiscellaneousSettingsModel
    {
        get { return _miscellaneousSettingsModel; }
        set { _miscellaneousSettingsModel = value; OnPropertyChanged(); }
    }
    #endregion

    #region Commands
    // Command to save the schedulerSetting.
    public RelayCommandHelper SaveSettingCommand { get; private set; }
    public RelayCommandHelper SaveCloudSettingCommand { get; private set; }
    // Command to save the setting related to Anonymization of patient name.
    public RelayCommandHelper SaveAnonymizedPatientNameCommand { get; private set; }
    // Command to save Save Aws Configuration
    public RelayCommandHelper SaveMiscellaneousConfigurationCommand { get; private set; }
    #endregion

    #region Methods
    public void GetMiscellaneousData_sync()
    {
        try
        {
            using (var repo = new GenericRepository<MiscellaneousModel>())
            {
                MiscellaneousSettingsModel = new MiscellaneousSettingsModel().GetMiscellaneousData_sync();
                MiscellaneousConfigurations = new MiscellaneousAWSConfigurationModel().GetMiscellaneousConfigurationDetails();
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
          
        }
    }

    #endregion

    #region IDataErrorInfo Members
    public string Error
    {
        get { throw new NotImplementedException(); }
    }
    private Dictionary<string, string> _errorCollection = new Dictionary<string, string>();
    public Dictionary<string, string> ErrorCollection
    {
        get { return _errorCollection; }
        set { _errorCollection = value; OnPropertyChanged(); }
    }

    #endregion

    

}






