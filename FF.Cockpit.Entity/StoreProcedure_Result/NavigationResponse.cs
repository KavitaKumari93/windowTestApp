using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml.Linq;
using Localisation = FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class NavigationResponse : PropertyChangeHelper
    {
        ObservableCollection<NavigationResponse> lst = null;
        ModulesModel modulesModel = null;
        #region Property
        public int Id { get; set; }
        public string Name { get; set; }
        private string iconKey;
        public string IconKey
        {
            get { return iconKey; }
            set { iconKey = value; OnPropertyChanged(); }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged(); }
        }
        #endregion


        #region Method
        public ObservableCollection<NavigationResponse> GetNavigationList()
        {
            try
            {
                var ModulesList = new ObservableCollection<NavigationResponse>();
                var assignedModules = new ModulePermissionModel().GetModuleDetailsByRoleID(AppStarter.ValidatedRoleId);
                ModulesList = assignedModules != null && assignedModules.Count() > 0 ?SetModulesInfo().Where(y => assignedModules.Any(z => z.ModuleId == y.Id && z.IsAccessible == true)).ToObservableCollection():SetModulesInfo();
                CheckModulesOverDb();

                return ModulesList;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        //Add Module listing to Db
        public void CheckModulesOverDb()
        {
            try
            {
                var modules = new ModulesModel().GetModuleNames();
                if (modules != null && modules.Count() > 0)
                {
                    // In case some new module is added to list,this will add the same to db
                    var data = lst.Select(x => x.Id).ToList().Except(modules.Select(x => x.ModuleId)).ToList();
                    if (data != null && data.Count() > 0)
                        SaveModuleToDb(data);
                }
                else
                    // To initially add the modules entry
                    SaveModuleToDb(SetModulesInfo().Select(x => x.Id).ToList());


                //Default entry for the Admin User
                if (new ModulePermissionModel().GetModuleDetailsByRoleID(1).Count() == 0)
                {
                    foreach (var item in lst)
                    {
                        ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                        modulePermissionModel.ModuleId = item.Id;
                        modulePermissionModel.UserRoleId = 1;
                        modulePermissionModel.IsAccessible = true;
                        new ModulePermissionModel().InsertOrUpdateModuleName(modulePermissionModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
        public void SaveModuleToDb(List<int> list)
        {
            try
            {
                if (list != null && list.Count() > 0)
                {
                    modulesModel = new ModulesModel();
                    foreach (var modules in list)
                    {
                        modulesModel.ModuleId = modules;
                        modulesModel.ModuleName = lst.Where(x => x.Id == (int)modules).FirstOrDefault().Name;
                        modulesModel.InsertOrUpdateModuleName(modulesModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }


        public ObservableCollection<NavigationResponse> SetModulesInfo()
        {
            lst = new ObservableCollection<NavigationResponse>();
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Operation, Name = Localisation.Operational_resx, IconKey = "Icon_Operation", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Scheduler, Name = Localisation.Scheduler_resx, IconKey = "Icon_Scheduler", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Dashboard, Name = Localisation.Dashboards_resx, IconKey = "Icon_Dashboard", IsSelected = true, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Performance, Name = Localisation.Performance_resx, IconKey = "Icon_Performance", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Configuration, Name = Localisation.Configuration_resx, IconKey = "Icon_Configuration", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Trends, Name = Localisation.Trends_resx, IconKey = "Icon_Trends", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.CloudSync, Name = Localisation.CloudSync_resx, IconKey = "Icon_CloudSync", IsSelected = false, IsActive = true });
            lst.Add(new NavigationResponse() { Id = (int)ModuleNames.Service, Name = Localisation.Service_resx, IconKey = "Icon_Service", IsSelected = false, IsActive = true });
            return lst;
        }
        #endregion
    }
}
