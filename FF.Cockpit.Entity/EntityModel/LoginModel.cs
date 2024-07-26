using FF.Cockpit.Common;

namespace FF.Cockpit.Entity.EntityModel
{
    public class LoginModel : PropertyChangeHelper
    {
        #region Properties
        private string serverInstance;
        public string ServerInstance
        {
            get { return serverInstance; }
            set { serverInstance = value; OnPropertyChanged(); }
        }
        private string databaseName;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; OnPropertyChanged(); }
        }
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; OnPropertyChanged(); }
        }
        private bool isSaveDbUserDetails;
        public bool IsSaveDbUserDetails
        {
            get { return isSaveDbUserDetails; }
            set { isSaveDbUserDetails = value; OnPropertyChanged(); }
        }
        private bool isSaveDbInstancesDetails;
        public bool IsSaveDbInstancesDetails
        {
            get { return isSaveDbInstancesDetails; }

            set { isSaveDbInstancesDetails = value; OnPropertyChanged(); }
        }

        private bool areDatabaseValuesLocked;
        public bool AreDatabaseValuesLocked
        {
            get { return areDatabaseValuesLocked; }
            set { areDatabaseValuesLocked = value; OnPropertyChanged(); }
        }
        private bool useWindowsSecurity;
        public bool UseWindowsSecurity
        {
            get { return useWindowsSecurity; }
            set { useWindowsSecurity = value; OnPropertyChanged(); }
        }
        private bool useSQLAuthenticate;
        public bool UseSQLAuthenticate
        {
            get { return useSQLAuthenticate; }
            set { useSQLAuthenticate = value; OnPropertyChanged(); }
        }
        public bool IsValidate()
        {
            bool isvalid = false;
            if (UseWindowsSecurity)
                isvalid = !string.IsNullOrEmpty(ServerInstance) && !string.IsNullOrEmpty(DatabaseName);
            else
                isvalid = !string.IsNullOrEmpty(ServerInstance) && !string.IsNullOrEmpty(DatabaseName) && (!string.IsNullOrEmpty(UserName));
            return isvalid;
        }
        #endregion
    }
}
