using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class UserLoginViewModel : PropertyChangeHelper
    {
        #region Constructor
        public UserLoginViewModel()
        {
            // Initialize UserLogin property with a new instance of UserLoginModel
            UserLogin = new UserLoginModel();

            // Initialize commands
            UserLoginCommand = new RelayCommandHelper(x => LoginMethod());
            ExitCommand = new RelayCommandHelper(x => ExitMethod());
            PasswordToggleCommand = new RelayCommandHelper(x => PasswordToggleMethod());
        }
        #endregion

        #region Properties
        // Property to hold user login information
        private UserLoginModel userLogin;
        public UserLoginModel UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; OnPropertyChanged(); }
        }

        // Property to toggle visibility of password
        private bool isMaskedPassword;
        public bool IsMaskedPassword
        {
            get { return isMaskedPassword; }
            set { isMaskedPassword = value; OnPropertyChanged(); }
        }
        #endregion

        #region Commands
        // Commands for user interactions
        public RelayCommandHelper ExitCommand { get; private set; }
        public RelayCommandHelper UserLoginCommand { get; private set; }
        public RelayCommandHelper PasswordToggleCommand { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Method to handle user login
        /// </summary>
        private void LoginMethod()
        {
            try
            {
                //// Check if username or password is empty
                if (string.IsNullOrWhiteSpace(UserLogin.UserName) || string.IsNullOrWhiteSpace(UserLogin.UserPassword))
                {
                    // Show error message if username or password is empty
                    CustomMessageBox.ShowOK(localisation.RequiredFieldPopUpMsg_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
                    return;
                }
                //// Check database connection and validate user
                if (DatabaseHelper.IsConnectionOpen((LoginModel)AppStarter.LoginModel) && ValidateUser())
                    NavigateToMainScreen();
            }
            catch (Exception ex)
            {
                // Show error message if exception occurs
                CustomMessageBox.ShowOK(ex.Message, localisation.ConnectionError_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
            }
        }
        /// <summary>
        ///  Method to validate user credentials
        /// </summary>
        /// <returns></returns>
        private bool ValidateUser()
        {
            // Get users data
            var usersData = new UsersInformationModel().GetUsersList_sync();
            if (usersData != null)
            {
                // Find user with matching email and password
                var user = usersData.ToObservableCollection().FirstOrDefault(x => x.Email == UserLogin.UserName && x.Password == UserLogin.UserPassword);
                if (user != null)
                {
                    // Set validated role ID and return true
                    AppStarter.ValidatedRoleId = user.UserRoleId;
                    return true;
                }
                else
                {
                    // Show error message if user is not found
                    CustomMessageBox.ShowOK(localisation.UserValidation_resx, localisation.DialogWindowAlertCaption_resx, localisation.OkBtn_resx, MessageBoxImage.Error);
                    return false;
                }
            }
            // Handle case when usersData is null
            else
                return false;
        }

        /// Method to handle application exit
        void ExitMethod()
        {
            // Close login window and open system login window
            if (AppStarter.LoginWindow != null)
            {
                Application.Current.MainWindow = new SystemLoginWindow();
                Application.Current.MainWindow?.Show();
                AppStarter.UserLoginWindow.Close();
            }
        }

        // Method to navigate to the main screen
        public void NavigateToMainScreen()
        {
            // Close login window and open main window
            if (AppStarter.LoginWindow != null)
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow?.Show();
                AppStarter.UserLoginWindow.Close();
            }
        }

        // Method to toggle password visibility
        public void PasswordToggleMethod()
        {
            IsMaskedPassword = !IsMaskedPassword ? true : false;
        }
        #endregion
    }
}
