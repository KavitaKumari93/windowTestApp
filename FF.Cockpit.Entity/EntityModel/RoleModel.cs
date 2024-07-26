using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using System.Collections.Generic;
using System;
using System.ComponentModel;


public class RoleModel : PropertyChangeHelper, IDataErrorInfo
{
    #region Properties
    private int _roleId;
    public int RoleId
    {
        get { return _roleId; }
        set { _roleId = value; OnPropertyChanged(); }
    }
    private string _roleName;
    public string RoleName
    {
        get { return _roleName; }
        set { _roleName = value; OnPropertyChanged(); }
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

    #region Constructor
    public RoleModel()
    {

    }
    #endregion
   
    #region Method
    public IEnumerable<RoleModel> GetRolesList_sync()
    {
        try
        {
            using (var repo = new GenericRepository<RoleModel>())
            {
                return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetRoles, new { });
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return null;
        }
    }

    public int InsertOrUpdateRoles_sync(RoleModel role)
    {
        try
        {
            using (var repo = new GenericRepository<RoleModel>())
            {
                var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateRoles,
                      new
                      {
                          @RoleId = role.RoleId,
                          @RoleName = role.RoleName.Trim(),
                          @IsActive = role.IsActive,

                      });
                return (model.RoleId);
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return 0;
        }
    }

    public int DeleteRole_sync(int roleId)
    {
        try
        {
            using (var repo = new GenericRepository<RoleModel>())
            {
                var data = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_DeleteRoles, new { @RoleId = roleId });
                return (data.RoleId);
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
                case "RoleName":
                    if (string.IsNullOrWhiteSpace(RoleName))
                        result = "Role name cannot be empty";

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
