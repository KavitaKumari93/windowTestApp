using FF.Cockpit.Common;
using FF.Cockpit.Entity;
using System.Collections.Generic;
using System;
using System.ComponentModel;


public class ModulePermissionModel : PropertyChangeHelper, IDataErrorInfo
{
    #region Properties

    private int _id;
    public int Id
    {
        get { return _id; }
        set { _id = value; OnPropertyChanged(); }
    }
    private int _userRoleId;
    public int UserRoleId
    {
        get { return _userRoleId; }
        set { _userRoleId = value; OnPropertyChanged(); }
    }
    private int _moduleId;
    public int ModuleId
    {
        get { return _moduleId; }
        set { _moduleId = value; OnPropertyChanged(); }
    }
    private bool _isAccessible;
    public bool IsAccessible
    {
        get { return _isAccessible; }
        set { _isAccessible = value; OnPropertyChanged(); }
    }
    

    #endregion

    #region Method
   
    public IEnumerable<ModulePermissionModel> GetModuleDetailsByRoleID(int _roleId)
    {
        try
        {
            using (var repo = new GenericRepository<ModulePermissionModel>())
            {
                return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetModulesPermissionInfoByRoleId, new{@roleId = _roleId });
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return null;
        }
    }

    public int InsertOrUpdateModuleName(ModulePermissionModel modulePermission)
    {
        try
        {
            using (var repo = new GenericRepository<ModulePermissionModel>())
            {
               
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateModulePermissionByRoleId,
                          new
                          {
                              //@id = modulePermission.Id,
                              @roleId = modulePermission.UserRoleId,
                              @moduleId = modulePermission.ModuleId,
                              @access= modulePermission.IsAccessible
                          });
              
                return 1;
            }
        }
        catch (Exception ex)
        {
            LogHelper.LogError(ex);
            return 0;
        }
    }

    public int DeleteModuleByRole_sync(int roleId)
    {
        try
        {
            using (var repo = new GenericRepository<ModulePermissionModel>())
            {
                var data = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_DeleteModulePermission, new { @RoleId = roleId });
                return (data.Id);
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
            //switch (columnName)
            //{
            //    case "RoleName":
            //        if (string.IsNullOrWhiteSpace(RoleName))
            //            result = "Role name cannot be empty";

            //        break;
            //}

            //if (ErrorCollection.ContainsKey(columnName))
            //    ErrorCollection[columnName] = result;
            //else if (result != null)
            //    ErrorCollection.Add(columnName, result);

            //if (result == null && ErrorCollection.ContainsKey(columnName))
            //    ErrorCollection.Remove(columnName);

            return result;
        }
    }
    #endregion


}
