using System;
using System.Collections.ObjectModel;
using FF.Cockpit.Common;
using localisation= FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.Entity.EntityModel
{
    public class ConfigurationModel : PropertyChangeHelper
    {
        #region Property

        private int _configurationId;
        public int ConfigurationId
        {
            get { return _configurationId; }
            set { _configurationId = value; OnPropertyChanged(); }
        }

        private string _configurationName;
        public string ConfigurationName
        {
            get { return _configurationName; }
            set { _configurationName = value; OnPropertyChanged(); }
        }

        private object _content;
        public object Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods 
        public ObservableCollection<ConfigurationModel> GetConfigurationList()
        {
            ObservableCollection<ConfigurationModel> lst = new ObservableCollection<ConfigurationModel>();
            try
            {
                using (var repo = new GenericRepository<ConfigurationModel>())
                {
                    lst.Add(new ConfigurationModel { ConfigurationId = 1, ConfigurationName = localisation.ConfigurationTabHeaderRoom_resx});
                    lst.Add(new ConfigurationModel { ConfigurationId = 2, ConfigurationName = localisation.ConfigurationTabHeaderUsersType_resx });
                    lst.Add(new ConfigurationModel { ConfigurationId = 3, ConfigurationName = localisation.ConfigurationTabHeaderRole_resx });
                    lst.Add(new ConfigurationModel { ConfigurationId = 4, ConfigurationName = localisation.ConfigurationTabHeaderPermission_resx });
                    lst.Add(new ConfigurationModel { ConfigurationId = 5, ConfigurationName = localisation.ConfigurationTabHeaderAppointmentTypes_resx });
                    lst.Add(new ConfigurationModel { ConfigurationId = 6, ConfigurationName = localisation.ConfigurationTabHeaderMiscellaneous_resx });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }

            return lst;
        }
        #endregion
    }

}

