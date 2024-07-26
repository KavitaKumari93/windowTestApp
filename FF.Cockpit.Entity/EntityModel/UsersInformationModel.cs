using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace FF.Cockpit.Entity.EntityModel
{
    public class UsersInformationModel : PropertyChangeHelper, IDataErrorInfo
    {
        #region Properties

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }
        private string _middleName;
        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; OnPropertyChanged(); }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
       
        private int userRoleId;
        public int UserRoleId
        {
            get{ return userRoleId<0 ? 1 : userRoleId;}
            set { userRoleId = value; OnPropertyChanged(); }
        }
        private byte[] _photo;
        public byte[] Photo
        {
            get { return _photo; }
            set { _photo = value; OnPropertyChanged(); }
        }
        private BitmapImage _photoSource = null;
        public BitmapImage PhotoSource
        {
            get { return _photoSource; }
            set { _photoSource = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        private string _specialization;
        public string Specialization
        {
            get { return _specialization; }
            set { _specialization = value; OnPropertyChanged(); }
        }
        private string _darkThemeColor;
        public string DarkThemeColor
        {
            get { return _darkThemeColor; }
            set { _darkThemeColor = value; OnPropertyChanged(); }
        }
        private string _lightThemeColor = string.Empty;
        public string LightThemeColor
        {
            get { return _lightThemeColor; }
            set { _lightThemeColor = value; OnPropertyChanged(); }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }
        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; OnPropertyChanged(); }
        }
        private DateTime _updatedDate;
        public DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; OnPropertyChanged(); }
        }
        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; OnPropertyChanged(); }
        }
        private string _updatedBy;
        public string UpdatedBy
        {
            get { return _updatedBy; }
            set { _updatedBy = value; OnPropertyChanged(); }
        }

        private object _content;
        public object Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }
        #endregion

        #region Method

        public async Task<IEnumerable<UsersInformationModel>> GetUsersList()
        {
            try
            {
                using (var repo = new GenericRepository<UsersInformationModel>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetUsersInformation, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public IEnumerable<UsersInformationModel> GetUsersList_sync()
        {
            try
            {
                using (var repo = new GenericRepository<UsersInformationModel>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetUsersInformation, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public int InsertOrUpdateUserInformation_sync(UsersInformationModel user)
        {
            try
            {
                using (var repo = new GenericRepository<UsersInformationModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateUserInformation,
                          new
                          {
                              @id = user.UserId,
                              @name = user.UserName.Trim(),
                              @userroleId= user.UserRoleId,
                              @phoneNumber = user.PhoneNumber,
                              @email = user.Email,
                              @password = user.Password,
                              @photo = user.Photo,
                              @specialization = user.Specialization,
                              @description = user.Description,
                              @lightThemeColor = user.LightThemeColor,
                              @darkThemeColor = user.DarkThemeColor,
                          });
                    return Convert.ToInt32(model.UserId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }
        public int DeleteUser_sync(int userId)
        {
            try
            {
                using (var repo = new GenericRepository<UsersInformationModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_DeleteUserByRoleId, new { @id = userId });
                    return Convert.ToInt32(model.UserId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }
        #endregion

        #region IDataErrorInfo Members
        string phonePattern = @"(\d)([() -]*)(\d)([() -]*)(\d)([() -]*)(\d)([() -]*)(\d)([() -]*)(\d)([() -]*)(\d+)"", ""x\\2x\\4x\\6x\\8x\\10x\\12\\13";
        // string phonePattern = @"\(?\+\(?49\)?[ ()]?([- ()]?\d[- ()]?){10}";

        string emailPattern = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";

        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
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
        public string this[string columnName]
        {
            get
          {
                string result = null;
                switch (columnName)
                {
                    case "UserName":
                        if (string.IsNullOrWhiteSpace(UserName))
                            result = "User name cannot be empty";
                        //else if (DoctorName.Length < 5)
                        //    result = "Doctor name must be a minimum of 5 characters.";
                        break;

                    case "LightThemeColor":
                        if (string.IsNullOrEmpty(LightThemeColor))
                            result = "Please select color";
                        break;

                    case "PhoneNumber":
                        if (!string.IsNullOrEmpty(PhoneNumber))
                        {
                            string numericPhone = new String(PhoneNumber.Where(Char.IsDigit).ToArray());
                            if (numericPhone.Length == 0 && ErrorCollection.ContainsKey(columnName))
                                ErrorCollection.Remove(columnName);
                            else if (numericPhone.Length < 12)
                                result = "Phone number is not valid";
                            
                            //if (!Regex.IsMatch(PhoneNumber, phonePattern))
                            //     result = "You have entered an invalid phone number!";
                        }
                        
                        break;

                    //case "PhotoSource":
                    //    if (PhotoSource == null)
                    //        result = "Please select image";
                    //    break;

                    case "Email":
                        //if (string.IsNullOrWhiteSpace(Email))
                        //    result = "Email cannot be empty";
                        if (!string.IsNullOrEmpty(Email) && !Regex.IsMatch(Email, emailPattern))
                            result = "You have entered an invalid email address!";
                        break;


                    case "Password":
                        //if (string.IsNullOrWhiteSpace(Password))
                        //    result = "Password cannot be empty";
                        if (!string.IsNullOrEmpty(Password) && !Regex.IsMatch(Password, passwordPattern))
                            result = "Password should have minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.";
                        break;
                    case "UserRoleId":
                        if (UserRoleId == 0)
                            result = "Assign Role For User";
                        break;
                }

                if (ErrorCollection.ContainsKey(columnName))
                    ErrorCollection[columnName] = result;
                else if (result != null)
                    ErrorCollection.Add(columnName, result);

                if (result == null && ErrorCollection.ContainsKey(columnName))
                    ErrorCollection.Remove(columnName);

                return result;
            }
        }
        #endregion
    }
}