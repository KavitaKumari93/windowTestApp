using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace FF.Cockpit.Entity.EntityModel
{
    public class RoomModel : PropertyChangeHelper, IDataErrorInfo
    {
        #region Properties
        public Brush ResourceBgColor { get; set; }

        private int _roomId;
        public int RoomId
        {
            get { return _roomId; }
            set { _roomId = value; OnPropertyChanged(); }
        }
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
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
        #endregion

        #region Method
        public async Task<IEnumerable<RoomModel>> GetRoomsList()
        {
            try
            {
                using (var repo = new GenericRepository<RoomModel>())
                {
                    var data = await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetRooms, new { });
                    Task.Delay(1000);
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public IEnumerable<RoomModel> GetRoomsList_sync()
        {
            try
            {
                using (var repo = new GenericRepository<RoomModel>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetRooms, new { });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public int InsertOrUpdateRoom_sync(RoomModel room)
        {
            try
            {
                using (var repo = new GenericRepository<RoomModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateRoom,
                          new
                          {
                              @id = room.RoomId,
                              @name = room.RoomName.Trim(),
                              @description = string.IsNullOrEmpty(room.Description) ? room.Description : room.Description.Trim(),
                              @lightThemeColor = room.LightThemeColor,
                              @darkThemeColor = room.DarkThemeColor,
                          });
                    return Convert.ToInt32(model.RoomId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }
        public int DeleteRoom_sync(int roomId)
        {
            try
            {
                using (var repo = new GenericRepository<RoomModel>())
                {
                    var data = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_DeleteRoom, new { @id = roomId });
                    return Convert.ToInt32(data.RoomId);
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
                    case "RoomName":
                        if (string.IsNullOrWhiteSpace(RoomName))
                            result = "Room name cannot be empty";
                        //else if (RoomName.Length < 5)
                        //    result = "Room name must be a minimum of 5 characters.";
                        break;

                    case "LightThemeColor":
                        if (string.IsNullOrEmpty(LightThemeColor))
                            result = "Please select color";
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