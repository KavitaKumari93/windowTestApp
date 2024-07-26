using Amazon.S3.Model;
using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.CustomControl;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.UserControls;
using FF.Cockpit.UI.Views.UserControls;
using FF.Cockpit.UI.Views.UserControls.Configuration;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.ViewModels
{
    public class ConfigurationViewModel : PropertyChangeHelper
    {
        public MainViewModel mainViewModel { get; set; }
        private Window addEditPopup;

        #region Properties 
        private bool _isAdd;
        public bool IsAdd
        {
            get { return _isAdd; }
            set { _isAdd = value; OnPropertyChanged(); }
        }
        // <summary>
        /// ///////////////Property to get the data from ConfigurationModel
        /// </summary>
        private ObservableCollection<ConfigurationModel> _configurationList;
        public ObservableCollection<ConfigurationModel> ConfigurationList
        {
            get { return _configurationList; }
            set { _configurationList = value; OnPropertyChanged(); }
        }


        private ConfigurationModel _currentConfiguration;
        public ConfigurationModel CurrentConfiguration
        {
            get { return _currentConfiguration; }
            set { _currentConfiguration = value; if (value != null) GetSelectedConfigurationView(); OnPropertyChanged(); }
        }

        // <summary>
        /// ///////////////Property to get the data from RoomModel
        /// </summary>
        private ObservableCollection<RoomModel> _roomList;
        public ObservableCollection<RoomModel> RoomList
        {
            get { return _roomList; }
            set { _roomList = value; }
        }
        // <summary>
        /// ///////////////Property to get the data from UserInformationModel
        /// </summary>
        private ObservableCollection<UsersInformationModel> _userList;
        public ObservableCollection<UsersInformationModel> UserList
        {
            get { return _userList; }
            set { _userList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<RoleModel> userRoleList;
        public ObservableCollection<RoleModel> UserRoleList
        {
            get { return userRoleList; }
            set { userRoleList = value; OnPropertyChanged(); }
        }

        // <summary>
        /// ///////////////Property to get the data from RoleModel
        /// </summary>
        private ObservableCollection<RoleModel> _roleList;
        public ObservableCollection<RoleModel> RoleList
        {
            get { return _roleList; }
            set { _roleList = value; OnPropertyChanged(); }
        }
        // <summary>
        /// ///////////////Property to get the data from AppointmentTypeModel
        /// </summary>
        private ObservableCollection<AppointmentTypeModel> _appointmentTypeList;
        public ObservableCollection<AppointmentTypeModel> AppointmentTypeList
        {
            get { return _appointmentTypeList; }
            set { _appointmentTypeList = value; OnPropertyChanged(); }
        }
        // <summary>
        /// ///////////////Property to get the data from RoomModel
        /// </summary>
        private RoomModel _selectedRoom;
        public RoomModel SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; }
        }
        // <summary>
        /// ///////////////Property to get the data from SelectedUser
        /// </summary>
        private UsersInformationModel _SelectedUser;
        public UsersInformationModel SelectedUser
        {
            get { return _SelectedUser; }
            set { _SelectedUser = value; OnPropertyChanged(); }
        }
        // <summary>
        /// ///////////////Property to get the data from SelectedRole
        /// </summary>
        private RoleModel _selectedRole;
        public RoleModel SelectedRole
        {
            get { return _selectedRole; }
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        // <summary>
        /// ///////////////Property to get the data from SelectedRoleId
        /// </summary>
        private int _selectedRoleId;
        public int SelectedRoleId
        {
            get { return _selectedRoleId; }
            set
            {
                _selectedRoleId = value;
                if (SelectedRoleId > 0)
                {
                    var roleinfonew = new ModulePermissionModel().GetModuleDetailsByRoleID(SelectedRoleId);
                    if (ModulesList == null || roleinfonew?.Count() == 0)
                        ModulesList = new ModulesModel().GetModuleNames().ToObservableCollection();
                    if (roleinfonew != null)
                    {
                        foreach (var items in roleinfonew)
                        {
                            ModulesList.Where(x => x.ModuleId == items.ModuleId).FirstOrDefault(x => x.IsActive = items.IsAccessible);
                        }
                    }

                }

            }
        }
        private AppointmentTypeModel _selectedAppointmentType;
        public AppointmentTypeModel SelectedAppointmentType
        {
            get { return _selectedAppointmentType; }
            set { _selectedAppointmentType = value; OnPropertyChanged(); }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged(); }
        }

        private MiscellaneousAWSConfigurationModel miscellaneousAWSConfiguration;
        public MiscellaneousAWSConfigurationModel MiscellaneousAWSConfiguration
        {
            get { return miscellaneousAWSConfiguration; }
            set { miscellaneousAWSConfiguration = value; OnPropertyChanged(); }
        }
        private bool isUserPasswordMasked;
        public bool IsUserPasswordMasked
        {
            get { return isUserPasswordMasked; }
            set { isUserPasswordMasked = value; OnPropertyChanged(); }
        }
        private bool isAWSAccessKeyMasked;
        public bool IsAWSAccessKeyMasked
        {
            get { return isAWSAccessKeyMasked; }
            set { isAWSAccessKeyMasked = value; OnPropertyChanged(); }
        }
        private bool isAWSSecretKeyMasked;
        public bool IsAWSSecretKeyMasked
        {
            get { return isAWSSecretKeyMasked; }
            set { isAWSSecretKeyMasked = value; OnPropertyChanged(); }
        }
        #region Modules

        private ObservableCollection<ModulesModel> modulesList;
        public ObservableCollection<ModulesModel> ModulesList
        {
            get { return modulesList; }
            set { modulesList = value; OnPropertyChanged(); }
        }

        private ModulesModel _selectedModule;
        public ModulesModel SelectedModule
        {
            get { return _selectedModule; }
            set { _selectedModule = value; OnPropertyChanged(); }
        }
        #endregion
        #endregion

        #region constructor
        public ConfigurationViewModel()
        {
            mainViewModel = (MainViewModel)AppStarter.mainVM;
            OpenPopupCommand = new RelayCommandHelper(x => OpenPopupMethod(x));
            OpenFileDialogCommand = new RelayCommandHelper(x => OpenFileDialogMethod());
            SaveUpdateDeleteCommand = new RelayCommandHelper(x => SaveUpdateDeleteMethod(x));
            CancelCommand = new RelayCommandHelper(x => CancelMethod());
            //CheckedCommand = new RelayCommandHelper(x => CheckBoxMethod());
            ConfigurationList = new ObservableCollection<ConfigurationModel>();
            ConfigurationList = new ConfigurationModel().GetConfigurationList();
            CurrentConfiguration = ConfigurationList.FirstOrDefault();

            SaveSettingCommand = new RelayCommandHelper(x => SaveSettingMethod());
            // SaveCloudSettingCommand = new RelayCommandHelper(x => SaveCloudSyncSettingMethod());
            SaveGeneralSettingsCommand = new RelayCommandHelper(x => SaveGeneralSettingMethod(x));
            SaveMiscellaneousConfigurationCommand = new RelayCommandHelper(x => SaveAWSConfigurationdetails());
            PasswordToggleCommand = new RelayCommandHelper(x => PasswordToggleMethod((string)x));
            GetRolesCommand = new RelayCommandHelper(x => GetRoles(x));
        }

        #endregion

        #region Commands
        // Command to save the schedulerSetting.
        public RelayCommandHelper SaveSettingCommand { get; private set; }
        public RelayCommandHelper SaveCloudSettingCommand { get; private set; }
        // Command to save the setting related to Anonymization of patient name.
        public RelayCommandHelper SaveGeneralSettingsCommand { get; private set; }
        // Command to open the Popup.
        public RelayCommandHelper OpenPopupCommand { get; private set; }
        // Command to open the FileDialogbutton on adding new doctor.
        public RelayCommandHelper OpenFileDialogCommand { get; private set; }
        // Command to open the Save,Update and Deletion of doctor,rolesand rooms .
        public RelayCommandHelper SaveUpdateDeleteCommand { get; private set; }
        // cancel button command
        public RelayCommandHelper CancelCommand { get; private set; }
        // Command to Save Aws Configuration
        public RelayCommandHelper SaveMiscellaneousConfigurationCommand { get; private set; }
        // Command to Show Password 
        public RelayCommandHelper PasswordToggleCommand { get; private set; }
        public RelayCommandHelper GetRolesCommand { get; private set; }
        #endregion

        #region Methods


        public void GetRoles(object selectedID)
        {
          
            SelectedRoleId = selectedID != null?(int)selectedID : RoleList.FirstOrDefault().RoleId;
            
        }
        // Method for get the selectedConfigurationView on Configuration schedular
        public void GetSelectedConfigurationView()
        {
            try
            {
                if (CurrentConfiguration != null)
                {
                    var task = Task.Factory.StartNew(() => { mainViewModel.LoaderShow(); });
                    task.ContinueWith(async (priorTask) =>
                    {
                        mainViewModel.TotalCallMethodCount++;

                        switch ((int)CurrentConfiguration.ConfigurationId)
                        {
                            case 1:
                                var roomData = await new RoomModel().GetRoomsList();
                                if (roomData != null) RoomList = roomData.ToObservableCollection();
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new RoomConfigurationView(this);
                                }));
                                break;
                            case 2:
                                var usersData = await new UsersInformationModel().GetUsersList();
                                if (usersData != null) UserList = usersData.ToObservableCollection();
                                {
                                    var role = new RoleModel().GetRolesList_sync();
                                    if (role != null) UserRoleList = role.ToObservableCollection();
                                }
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new UserConfigurationView(this);
                                }));
                                break;
                            case 3:
                                var roledata = new RoleModel().GetRolesList_sync();
                                if (roledata != null) RoleList = roledata.ToObservableCollection();
                             
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new RoleConfigurationView(this);
                                }));
                                break;
                            case 4:
                                if (RoleList == null)
                                {
                                    var roledataForPermission = new RoleModel().GetRolesList_sync();
                                    if (roledataForPermission != null) RoleList = roledataForPermission.ToObservableCollection();
                                    SelectedRoleId = RoleList.FirstOrDefault().RoleId;
                                }
                                if (RoleList != null && RoleList.Count() > 0)
                                {
                                    if (SelectedRoleId == 0)
                                        SelectedRoleId = RoleList.FirstOrDefault().RoleId;
                                }
                                if (SelectedRoleId > 0)
                                {
                                    var roleinfonew = new ModulePermissionModel().GetModuleDetailsByRoleID(SelectedRoleId);
                                    if (ModulesList == null || roleinfonew?.Count() == 0)
                                        ModulesList = new ModulesModel().GetModuleNames().ToObservableCollection();
                                    if (roleinfonew != null)
                                    {
                                        foreach (var items in roleinfonew)
                                        {
                                            ModulesList.Where(x => x.ModuleId == items.ModuleId).FirstOrDefault(x => x.IsActive = items.IsAccessible);
                                        }
                                    }
                                }
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new RolePermissionConfigurationView(this);
                                }));
                                break;
                            case 5:
                                var appointmentTypedata = new AppointmentTypeModel().GetAppointmentTypesList_sync();
                                if (appointmentTypedata != null) AppointmentTypeList = appointmentTypedata.ToObservableCollection();
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new AppointmentTypeConfigurationView(this);
                                }));
                                break;
                            case 6:
                                var startHourObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.StartHour.ToString())?.Value;
                                SelectedStartHour = startHourObj != null ? Convert.ToDouble(startHourObj) : 360;

                                var endHourObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.EndHour.ToString())?.Value;
                                SelectedEndHour = endHourObj != null ? Convert.ToDouble(endHourObj) : 1200;

                                var dayOfWeekObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.FirstDayOfWeek.ToString())?.Value;
                                if (dayOfWeekObj != null)
                                {
                                    DayOfWeek dayOfWeek;
                                    Enum.TryParse(dayOfWeekObj.ToString(), out dayOfWeek);

                                    SelectedDayOfWeek = dayOfWeek;
                                }
                                else
                                    SelectedDayOfWeek = DayOfWeek.Monday;

                                //var cloudSyncMenuVisibleObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsCloudSyncMenuVisible.ToString())?.Value;
                                //IsCloudSyncMenuVisible = cloudSyncMenuVisibleObj != null && Convert.ToInt32(cloudSyncMenuVisibleObj) == 1 ? true : false;

                                //var serviceMenuVisibleObj = AppointmentRefData.miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.IsServiceMenuVisible.ToString())?.Value;
                                //Service = cloudSyncMenuVisibleObj != null && Convert.ToInt32(serviceMenuVisibleObj) == 1 ? true : false;


                                MiscellaneousAWSConfiguration = new MiscellaneousAWSConfigurationModel();
                                {
                                    MiscellaneousAWSConfiguration = MiscellaneousAWSConfiguration.GetMiscellaneousConfigurationDetails()?.SingleOrDefault(x => x.IsActive == true);
                                    if (MiscellaneousAWSConfiguration == null)
                                    {
                                        MiscellaneousAWSConfiguration = new MiscellaneousAWSConfigurationModel();
                                        MiscellaneousAWSConfiguration.AWSConfigSubmitText = localisation.SaveButton_resx;
                                    }
                                    else
                                        MiscellaneousAWSConfiguration.AWSConfigSubmitText = localisation.UpdateButton_resx;

                                }
                                App.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() =>
                                {
                                    CurrentConfiguration.Content = new MiscellaneousConfigurationView(this);

                                }));
                                break;

                        }
                        mainViewModel.CalledMethodCount++;
                    });
                    SelectedRoleId = 0;
                    task.ContinueWith((antecedent) => mainViewModel.LoaderHide(mainViewModel.TotalCallMethodCount));
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        // Method for Open the PopUp For adding and editing roles,rooms and doctors
        private void OpenPopupMethod(object isAddButtonClick)
        {

            IsAdd = isAddButtonClick != null ? true : false;
            try
            {
                if (IsAdd)
                {
                    SelectedRoom = new RoomModel();
                    SelectedUser = new UsersInformationModel();
                    SelectedRole = new RoleModel();
                    SelectedAppointmentType = new AppointmentTypeModel();
                }
                else
                {
                    if (SelectedUser != null && SelectedUser.Photo != null)
                        SelectedUser.PhotoSource = SelectedUser.Photo.GetBitmapImageFromBytes();
                }


                if (CurrentConfiguration.ConfigurationId == 1)
                    addEditPopup = new AddRoom(this);
                else if (CurrentConfiguration.ConfigurationId == 2)
                    addEditPopup = new AddUser(this);
                else if (CurrentConfiguration.ConfigurationId == 3)
                    addEditPopup = new AddRole(this);
                else if (CurrentConfiguration.ConfigurationId == 5)
                    addEditPopup = new AddAppointmentType(this);


                if (addEditPopup != null)
                {
                    addEditPopup.Owner = AppStarter.MainWindow;
                    addEditPopup.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        // method for OpenFileDialog 
        private void OpenFileDialogMethod()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Image Files | *.jpg; *.jpeg; *.png";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var imagePath = openFileDialog1.FileName;
                    if (!string.IsNullOrWhiteSpace(imagePath))
                    {
                        SelectedUser.PhotoSource = imagePath.GetBitmapImageFromImagePath();
                        SelectedUser.Photo = imagePath.GetBytesFromImagePath();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        // method for save,update and delete the Rooms,doctors and roles
        private void SaveUpdateDeleteMethod(object isDeleteButtonClick)
        {
            try
            {
                int returnValue = 0;
                int errorCount = 0;
                bool isDuplicate = false;

                if (CurrentConfiguration.ConfigurationId == 1)
                {
                    if (SelectedRoom != null)
                    {
                        errorCount = SelectedRoom.ErrorCollection.Count();
                        if (isDeleteButtonClick != null)
                            returnValue = SelectedRoom.DeleteRoom_sync(SelectedRoom.RoomId);
                        else if (errorCount == 0)
                        {
                            var list = new RoomModel().GetRoomsList_sync();

                            if (list != null && list.Count() > 0)
                            {
                                list.ToObservableCollection();

                                if (SelectedRoom.RoomId > 0)
                                    list = list.Where(f => f.RoomId != SelectedRoom.RoomId).ToList();

                                isDuplicate = list.Where(x => x.RoomName.ToLower().Trim() == SelectedRoom.RoomName.ToLower().Trim()).Count() > 0;
                                //isDuplicate = list.Where(x => !string.IsNullOrWhiteSpace(SelectedRoom?.RoomName) && x.RoomName == SelectedRoom.RoomName).Count() > 0;
                            }

                            if (isDuplicate)
                                CustomMessageBox.ShowOK(localisation.RoomNameAlreadyExists_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Warning);
                            else
                                returnValue = SelectedRoom.InsertOrUpdateRoom_sync(SelectedRoom);
                        }
                        GetSelectedConfigurationView();
                    }

                }

                else if (CurrentConfiguration.ConfigurationId == 2)
                {
                    if (SelectedUser != null)
                    {
                        errorCount = SelectedUser.ErrorCollection.Count();
                        if (isDeleteButtonClick != null)
                            returnValue = SelectedUser.DeleteUser_sync(SelectedUser.UserId);
                        else if (errorCount == 0)
                        {
                            var list = new UsersInformationModel().GetUsersList_sync();

                            if (list != null && list.Count() > 0)
                            {
                                list.ToObservableCollection();

                                if (SelectedUser.UserId > 0)
                                    list = list.Where(f => f.UserId != SelectedUser.UserId).ToList();
                                isDuplicate = list.Where(x => x.UserName.ToLower().Trim() == SelectedUser.UserName.ToLower().Trim()).Count() > 0;
                            }
                            if (isDuplicate)
                                CustomMessageBox.ShowOK(localisation.DoctorNameAlreadyExists_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Warning);
                            else
                            {
                                returnValue = SelectedUser.InsertOrUpdateUserInformation_sync(SelectedUser);
                            }
                        }
                        GetSelectedConfigurationView();
                    }

                }

                else if (CurrentConfiguration.ConfigurationId == 3)
                {
                    if (SelectedRole != null)
                    {
                        errorCount = SelectedRole.ErrorCollection.Count();
                        if (isDeleteButtonClick != null)
                            returnValue = SelectedRole.DeleteRole_sync(SelectedRole.RoleId);
                        else if (errorCount == 0)
                        {
                            var list = new RoleModel().GetRolesList_sync();

                            if (list != null && list.Count() > 0)
                            {
                                list.ToObservableCollection();

                                if (SelectedRole.RoleId > 0)
                                    list = list.Where(f => f.RoleId != SelectedRole.RoleId).ToList();
                                isDuplicate = list.Where(x => x.RoleName.ToLower().Trim() == SelectedRole.RoleName.ToLower().Trim()).Count() > 0;
                                //isDuplicate = list.Where(x => x.RoleName == SelectedRole.RoleName).Count() > 0;
                            }
                            if (isDuplicate)
                                CustomMessageBox.ShowOK(localisation.RoleNameAlreadyExists_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Warning);
                            else
                            {
                                returnValue = SelectedRole.InsertOrUpdateRoles_sync(SelectedRole);

                            }
                        }

                        GetSelectedConfigurationView();
                    }

                }

                else if (CurrentConfiguration.ConfigurationId == 4)
                {
                    if (isDeleteButtonClick.ToString() == "SaveClick")
                    {
                        //to do : save permisson w.r.t selected role
                        foreach (var item in ModulesList)
                        {
                            ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                            modulePermissionModel.ModuleId = item.ModuleId;
                            modulePermissionModel.UserRoleId = SelectedRoleId;
                            modulePermissionModel.IsAccessible = item.IsActive;
                            new ModulePermissionModel().InsertOrUpdateModuleName(modulePermissionModel);

                        }
                        CustomMessageBox.ShowOK(localisation.SavedSuccessfullyMsg_resx, localisation.SuccessMsg_resx, localisation.OkBtn_resx, MessageBoxImage.Information);
                        ////SelectedRoleId = 0;
                        //GetSelectedConfigurationView();
                        //SelectedRole = RoleList.FirstOrDefault(x => x.RoleId == SelectedRoleId);

                    }
                    else if (isDeleteButtonClick.ToString() == "PermissionClick")
                    { 
                        var result = RoleList.FirstOrDefault(x => x.RoleName.ToLower() == UserType.Admin.ToString().ToLower());
                        if (result != null)
                        {
                            if (SelectedRoleId == result.RoleId)
                            {
                                if (new ModulePermissionModel().GetModuleDetailsByRoleID(SelectedRoleId).Count() == 0 )
                                {
                                    foreach (var item in new NavigationResponse().SetModulesInfo().ToList())
                                    {
                                        ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                                        modulePermissionModel.ModuleId = item.Id;
                                        modulePermissionModel.UserRoleId = SelectedRoleId;
                                        modulePermissionModel.IsAccessible = true;
                                        new ModulePermissionModel().InsertOrUpdateModuleName(modulePermissionModel);
                                    }
                                }
                            }
                            else
                            {
                                ModulesList.FirstOrDefault(x => x.ModuleId == SelectedModule.ModuleId).IsActive = !SelectedModule.IsActive;
                            }
                        }
                        

                    }

                }

                else if (CurrentConfiguration.ConfigurationId == 5)
                {
                    if (SelectedAppointmentType != null)
                    {
                        errorCount = SelectedAppointmentType.ErrorCollection.Count();
                        if (isDeleteButtonClick != null)
                            returnValue = SelectedAppointmentType.DeleteAppointmentType_sync(SelectedAppointmentType.AppointmentTypeId);
                        else if (errorCount == 0)
                        {
                            var list = new AppointmentTypeModel().GetAppointmentTypesList_sync();

                            if (list != null && list.Count() > 0)
                            {
                                list.ToObservableCollection();

                                if (SelectedAppointmentType.AppointmentTypeId > 0)
                                    list = list.Where(f => f.AppointmentTypeId != SelectedAppointmentType.AppointmentTypeId).ToList();
                                isDuplicate = list.Where(x => x.AppointmentTypeName.ToLower().Trim() == SelectedAppointmentType.AppointmentTypeName.ToLower().Trim()).Count() > 0;
                                //isDuplicate = list.Where(x => x.AppointmentTypeName == SelectedAppointmentType.AppointmentTypeName || x.AppointmentTypeColor == SelectedAppointmentType.AppointmentTypeColor).Count() > 0;
                            }
                            if (isDuplicate)
                                CustomMessageBox.ShowOK(localisation.AppointmentColorAlreadyExists_resx, localisation.Warning_resx, localisation.OkBtn_resx, MessageBoxImage.Warning);
                            else
                                returnValue = SelectedAppointmentType.InsertOrUpdateAppointmentTypes_sync(SelectedAppointmentType);

                        }
                        GetSelectedConfigurationView();
                    }
                }



                if (errorCount == 0 && addEditPopup != null && !isDuplicate)
                    addEditPopup.Close();
                else
                    errorCount = 0;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        private void CancelMethod()
        {
            try
            {
                addEditPopup.Close();
                GetSelectedConfigurationView();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        #endregion

        #region Scheduler Setttings

        private double _selectedStartHour;
        public double SelectedStartHour
        {
            get { return _selectedStartHour; }
            set { _selectedStartHour = value; OnPropertyChanged(); }
        }
        private double _selectedEndHour;
        public double SelectedEndHour
        {
            get { return _selectedEndHour; }
            set { _selectedEndHour = value; OnPropertyChanged(); }
        }
        private DayOfWeek _selectedDayOfWeek;
        public DayOfWeek SelectedDayOfWeek
        {
            get { return _selectedDayOfWeek; }
            set { _selectedDayOfWeek = value; OnPropertyChanged(); }
        }

        private int _syncCloudDays;
        public int SyncCloudDays
        {
            get { return _syncCloudDays; }
            set { _syncCloudDays = value; OnPropertyChanged(); }
        }
        #endregion

        private bool _isCloudSyncMenuVisible;
        public bool IsCloudSyncMenuVisible
        {
            get { return _isCloudSyncMenuVisible; }
            set { _isCloudSyncMenuVisible = value; OnPropertyChanged(); }
        }
        private bool _isPatientNameAnonymizedSetting;
        public bool IsPatientNameAnonymizedSetting
        {
            get { return _isPatientNameAnonymizedSetting; }
            set { _isPatientNameAnonymizedSetting = value; OnPropertyChanged(); }
        }

        private void SaveSettingMethod()
        {
            try
            {
                foreach (var MiscellaneousSettingsModel in AppointmentRefData.miscellaneousData)
                {
                    MiscellaneousKeys miscellaneousKey;
                    Enum.TryParse(MiscellaneousSettingsModel.Name, out miscellaneousKey);

                    switch (miscellaneousKey)
                    {
                        case MiscellaneousKeys.StartHour:
                            if (MiscellaneousSettingsModel.Value == null || Convert.ToDouble(MiscellaneousSettingsModel.Value) != SelectedStartHour)
                            {
                                MiscellaneousSettingsModel.Value = SelectedStartHour;
                                new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                            }
                            break;
                        case MiscellaneousKeys.EndHour:
                            if (MiscellaneousSettingsModel.Value == null || Convert.ToDouble(MiscellaneousSettingsModel.Value) != SelectedEndHour)
                            {
                                MiscellaneousSettingsModel.Value = SelectedEndHour;
                                new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                            }
                            break;
                        case MiscellaneousKeys.FirstDayOfWeek:
                            if (MiscellaneousSettingsModel.Value == null)
                            {
                                MiscellaneousSettingsModel.Value = SelectedDayOfWeek.ToString();
                                new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                            }
                            else
                            {
                                DayOfWeek dayOfWeek;
                                Enum.TryParse(MiscellaneousSettingsModel.Value?.ToString(), out dayOfWeek);

                                if (dayOfWeek.ToString() != SelectedDayOfWeek.ToString())
                                {
                                    MiscellaneousSettingsModel.Value = SelectedDayOfWeek.ToString();
                                    new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                                }
                            }
                            break;
                        case MiscellaneousKeys.SyncCloudDays:
                            if ((MiscellaneousSettingsModel.Value == null || Convert.ToInt32(MiscellaneousSettingsModel.Value) != SyncCloudDays) && SyncCloudDays > 0)
                            {
                                MiscellaneousSettingsModel.Value = SyncCloudDays;
                                new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                            }
                            break;
                        case MiscellaneousKeys.IsCloudSyncMenuVisible:
                            //MiscellaneousSettingsModel.Value = IsCloudSyncMenuVisible;
                            //new MiscellaneousSettingsModel().InsertOrUpdateMiscellaneousData(MiscellaneousSettingsModel);
                            //var viewModel = (MainViewModel)AppStarter.mainVM;
                            //if (IsCloudSyncMenuVisible && viewModel.NavigationModelList.FirstOrDefault(x => x.Name == ModuleNames.CloudSync.ToString()) == null)
                            //    viewModel.NavigationModelList.Add(new NavigationResponse() { Id = (int)ModuleNames.CloudSync, Name = localisation.CloudSync_resx, IconKey = "Icon_CloudSync", IsSelected = false });
                            //else
                            //    viewModel.NavigationModelList.Remove(viewModel.NavigationModelList.Where(x => x.Id == (int)ModuleNames.CloudSync).FirstOrDefault());

                            break;

                    }
                }
                var data = new MiscellaneousSettingsModel().GetMiscellaneousData_sync();
                if (data != null && data.Count() > 0)
                {
                    AppointmentRefData.miscellaneousData = data.ToObservableCollection();
                    GetSelectedConfigurationView();
                }
                CustomMessageBox.ShowOK($"{localisation.SavedSuccessfullyMsg_resx} !", localisation.SuccessMsg_resx, localisation.OkBtn_resx, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        /// <summary>
        /// To Save Setting to display patients in Anonymized form or not under MiscellaneousData Table 
        /// in a case of updation or new entry to db 
        /// </summary>
        /// <param name="isChecked"></param>
        private void SaveGeneralSettingMethod(object checkbox)
        {
            try
            {
                if (checkbox != null)
                {
                    var checkboxDetails = (System.Windows.Controls.CheckBox)checkbox;
                    bool? IsChecked = checkboxDetails.IsChecked;
                    string miscKeys = string.Empty;
                    CheckBoxTag selectedCheckbox;
                    Enum.TryParse<CheckBoxTag>((string)checkboxDetails.Tag, out selectedCheckbox);
                    switch (selectedCheckbox)
                    {
                        case CheckBoxTag.Anonymized:
                            miscKeys = MiscellaneousKeys.IsPatientsNameAnonymized.ToString();
                            AppStarter.IsPatientNameAnontmized = IsChecked.Value;
                            break;
                        case CheckBoxTag.ServiceTab:
                            //miscKeys = MiscellaneousKeys.IsServiceMenuVisible.ToString();
                            //var viewModel = (MainViewModel)AppStarter.mainVM;
                            //if (IsChecked == true && viewModel.NavigationModelList.FirstOrDefault(x => x.Name == ModuleNames.Service.ToString()) == null)
                            //    viewModel.NavigationModelList.Add(new NavigationResponse() { Id = (int)ModuleNames.Service, Name = localisation.Service_resx, IconKey = "Icon_Service", IsSelected = false, IsActive = true });
                            //else
                            //    viewModel.NavigationModelList.Remove(viewModel.NavigationModelList.Where(x => x.Id == (int)ModuleNames.Service).FirstOrDefault());
                            break;
                    }
                    MiscellaneousSettingsModel miscellaneous = null;
                    var miscData = new MiscellaneousSettingsModel().GetMiscellaneousData_sync();
                    if (miscData != null && miscData.Count() > 0)
                    {
                        miscellaneous = new MiscellaneousSettingsModel();
                        miscellaneous.Id = miscData.FirstOrDefault(x => x.Name == miscKeys) == null ? 0 : miscData.FirstOrDefault(x => x.Name == miscKeys).Id;
                        miscellaneous.Name = miscKeys;
                        miscellaneous.Value = IsChecked;
                        miscellaneous.InsertOrUpdateMiscellaneousData(miscellaneous);
                    }
                }
            }

            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public void SaveAWSConfigurationdetails()
        {
            try
            {
                if (MiscellaneousAWSConfiguration != null)
                {
                    if (!String.IsNullOrEmpty(MiscellaneousAWSConfiguration.AWSBucketName) && !String.IsNullOrEmpty(MiscellaneousAWSConfiguration.AWSAccessKey) && !String.IsNullOrEmpty(MiscellaneousAWSConfiguration.AWSSecretKey) && !String.IsNullOrEmpty(MiscellaneousAWSConfiguration.AWSBucketRegion))
                    {
                        MiscellaneousAWSConfiguration.Id = MiscellaneousAWSConfiguration.InsertMiscellaneousConfigurationData(MiscellaneousAWSConfiguration);
                        if (MiscellaneousAWSConfiguration.Id > 0)
                            CustomMessageBox.ShowOK($"{localisation.SavedSuccessfullyMsg_resx} !", localisation.SuccessMsg_resx, localisation.OkBtn_resx, MessageBoxImage.Information);
                    }
                    else
                        CustomMessageBox.Show(localisation.RequiredFieldPopUpMsg_resx, localisation.DialogWindowAlertCaption_resx, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

        public void PasswordToggleMethod(string keyType)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyType))
                {
                    if (keyType == "AccessKey")
                        IsAWSAccessKeyMasked = !IsAWSAccessKeyMasked ? true : false;
                    else if (keyType == "UserPassword")
                        IsUserPasswordMasked = !IsUserPasswordMasked ? true : false;
                    else
                        IsAWSSecretKeyMasked = !IsAWSSecretKeyMasked ? true : false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }

    }
}
