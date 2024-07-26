using FF.Cockpit.Common;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Xml.Linq;
using System.IO;
using FF.Cockpit.Entity;
using FF.Cockpit.Entity.EntityModel;
using System.Data.Common;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using FF.Cockpit.UI.Helpers;
using System.Xml;
using System.Xaml;
using System.Windows.Controls;
using Amazon.S3.Model;
using localisation = FF.Cockpit.Common.Properties.Resources;
using FF.Cockpit.UI.CustomControl;

namespace FF.Cockpit.UI.ViewModels
{
    public class SystemLoginViewModel : PropertyChangeHelper
    {
        #region Constructor
        public SystemLoginViewModel()
        {
            Login = new LoginModel();
            LoginCommand = new RelayCommandHelper(x => { LoginMethod(); });
            ExitCommand = new RelayCommandHelper(x => { ExitMethod(); });
            SaveDbLoginCommand = new RelayCommandHelper(x => { SaveXMLDetails(); });
            LockCommand = new RelayCommandHelper(x => { LockMethod(); });
            AuthenticateSQLCommand = new RelayCommandHelper(x => { AuthenticateSQLMethod(); });
            AuthenticateWindowCommand = new RelayCommandHelper(x => { AuthenticateWindowMethod(); });
            UserLoginCommand = new RelayCommandHelper(x => { UserLoginScreen(); });
            OnLoad();
        }
        #endregion 

        #region Properties
        private LoginModel login;
        public LoginModel Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        public RelayCommandHelper ExitCommand { get; set; }
        public RelayCommandHelper LoginCommand { get; set; }
        public RelayCommandHelper SaveDbLoginCommand { get; set; }
        public RelayCommandHelper LockCommand { get; set; }
        public RelayCommandHelper AuthenticateWindowCommand { get; set; }
        public RelayCommandHelper AuthenticateSQLCommand { get; set; }
        public RelayCommandHelper UserLoginCommand { get; set; }

        #endregion

        #region Methods

        private void LockMethod()
        {
            Login.AreDatabaseValuesLocked = !Login.AreDatabaseValuesLocked;
        }

        public void AssignValues()
        {
            Login.UseWindowsSecurity = true; Login.UseSQLAuthenticate = false;
            Login.UserName = string.Empty; Login.UserPassword = string.Empty; Login.IsSaveDbUserDetails = false;
        }
        private void AuthenticateWindowMethod()
        {
            AssignValues();
        }

        private void AuthenticateSQLMethod()
        {
            Login.UseWindowsSecurity = false;
            if (Login.IsSaveDbUserDetails)
            {
                Login.UserName = CommonMethods.ReadXMLDoc("UserName");
                Login.UserPassword = CommonMethods.ReadXMLDoc("UserPassword");
            }
        }
        private void LoginMethod()
        {
            try
            {
                if (!Login.UseWindowsSecurity) 
                {
                    if (Login.IsValidate())
                    {
                        AppStarter.ValidatedRoleId = 1;
                        Navigate();
                    }
                    else
                        CustomMessageBox.ShowOK(localisation.RequiredFieldPopUpMsg_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
                }
                else
                    Navigate();
                
            }
            catch (Exception ex)
            {
                CustomMessageBox.ShowOK(ex.Message, localisation.ConnectionError_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
            }

        }
        public void Navigate()
        {
            if (DatabaseHelper.IsConnectionOpen(Login))
            {
                if (Login.AreDatabaseValuesLocked)
                    CommonMethods.WriteUnderXMLDoc("IsSaveDatabaseServerDetails", Login.AreDatabaseValuesLocked);

                CommonMethods.WriteUnderXMLDoc("DataSource", Login.ServerInstance);
                CommonMethods.WriteUnderXMLDoc("InitialCatalog", Login.DatabaseName);

                CommonMethods.WriteUnderXMLDoc("IsWindowsAuthenticationOn", Login.UseWindowsSecurity);
                if (Login.IsSaveDbInstancesDetails)
                {
                    CommonMethods.WriteUnderXMLDoc("UserName", Login.UserName);
                    CommonMethods.WriteUnderXMLDoc("UserPassword", Login.UserPassword);
                }
                NavigateToMainScreen();
            }
            else
                CustomMessageBox.ShowOK(localisation.DbConnectionErrorMsg_resx, localisation.ConnectionError_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
        }

        public void NavigateToMainScreen()
        {

            if (AppStarter.LoginWindow != null)
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow?.Show();
                AppStarter.LoginWindow.Close();
            }
        }

        void ExitMethod()
        {
            Application.Current.Shutdown();
        }

        public void OnLoad()
        {

            Login.IsSaveDbInstancesDetails = Convert.ToBoolean(CommonMethods.ReadXMLDoc("IsSaveDatabaseServerDetails"));
            Login.IsSaveDbUserDetails = Convert.ToBoolean(CommonMethods.ReadXMLDoc("IsSaveDatabseUserDetails"));
            Login.UseWindowsSecurity = Convert.ToBoolean(CommonMethods.ReadXMLDoc("IsWindowsAuthenticationOn"));
            Login.UseSQLAuthenticate = !Login.UseWindowsSecurity;
            if (Login.UseWindowsSecurity)
                AssignValues();
            //Assign Server Instance
            Login.ServerInstance = Login.IsSaveDbInstancesDetails ? CommonMethods.ReadXMLDoc("DataSource") : string.Empty;
            Login.DatabaseName = Login.IsSaveDbInstancesDetails ? CommonMethods.ReadXMLDoc("InitialCatalog") : string.Empty;
            //Assign User Details
            Login.UserName = Login.IsSaveDbUserDetails ? CommonMethods.ReadXMLDoc("UserName") : string.Empty;
            Login.UserPassword = Login.IsSaveDbUserDetails ? CommonMethods.ReadXMLDoc("UserPassword") : string.Empty;

            Login.AreDatabaseValuesLocked = true;
        }

        public void SaveXMLDetails()
        {
            CommonMethods.WriteUnderXMLDoc("IsSaveDatabseUserDetails", Login.IsSaveDbUserDetails);
        }

        public void UserLoginScreen()
        {
            try
            {
                if (AppStarter.LoginModel == null)
                {
                    AppStarter.LoginModel = new LoginModel();
                    AppStarter.LoginModel = Login;
                }
                Application.Current.MainWindow = new UserLogin();
                Application.Current.MainWindow?.Show();
                AppStarter.LoginWindow.Close();

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        #endregion
    }
}

