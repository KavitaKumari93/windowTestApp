using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using System.Collections.Generic;
using System;
using System.ComponentModel;


public class AppointmentTypeModel : PropertyChangeHelper, IDataErrorInfo
{
    #region Properties
    private int _AppointmentTypeId;
    public int AppointmentTypeId
    {
        get { return _AppointmentTypeId; }
        set { _AppointmentTypeId = value; OnPropertyChanged(); }
    }
    private string _AppointmentTypeName;
    public string AppointmentTypeName
    {
        get { return _AppointmentTypeName; }
        set { _AppointmentTypeName = value; OnPropertyChanged(); }
    }
    private string _appointmentTypeColor;
    public string AppointmentTypeColor
    {
        get { return _appointmentTypeColor; }
        set { _appointmentTypeColor = value; OnPropertyChanged(); }
    }
    
    private bool _isDeleted;
    public bool IsDeleted
    {
        get { return _isDeleted; }
        set { _isDeleted = value; OnPropertyChanged(); }
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
    public IEnumerable<AppointmentTypeModel> GetAppointmentTypesList_sync()
    {
        try
        {
            using (var repo = new GenericRepository<AppointmentTypeModel>())
            {
                return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetAppointmentTypes, new { });
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return null;
        }
    }

    public int InsertOrUpdateAppointmentTypes_sync(AppointmentTypeModel AppointmentType)
    {
        try
        {
            using (var repo = new GenericRepository<AppointmentTypeModel>())
            {
                var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateAppointmentType,
                      new
                      {
                          @appointmentTypeId = AppointmentType.AppointmentTypeId,
                          @appointmentTypeName = AppointmentType.AppointmentTypeName.Trim(),
                          @appointmentTypeColor = AppointmentType.AppointmentTypeColor,
                      });
                return (model.AppointmentTypeId);
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return 0;
        }
    }

    public int DeleteAppointmentType_sync(int AppointmentTypeId)
    {
        try
        {
            using (var repo = new GenericRepository<AppointmentTypeModel>())
            {
                var data = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_DeleteAppointmentType, new { @appointmentTypeId = AppointmentTypeId });
                return (data.AppointmentTypeId);
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
                case "AppointmentTypeName":
                    if (string.IsNullOrWhiteSpace(AppointmentTypeName))
                        result = "AppointmentType name cannot be empty";

                    break;

                case "AppointmentTypeColor":
                    if (string.IsNullOrEmpty(AppointmentTypeColor))
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
