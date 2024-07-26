using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.UserControls.CloudSync
{
    /// <summary>
    /// Interaction logic for AppointedPatientsSyncTile.xaml
    /// </summary>
    public partial class AppointedPatientsSyncTile : UserControl
    {
        private CloudSyncViewModel cloudSyncViewModel;
        MainViewModel vm = null;
        public AppointedPatientsSyncTile()
        {
            InitializeComponent();
            dataGrid.SelectedIndex = 0;
            vm = AppStarter.mainVM as MainViewModel;
            MainViewModel mainViewModel = (MainViewModel)AppStarter.mainVM;
            cloudSyncViewModel = (CloudSyncViewModel)mainViewModel.CurrentView;
            this.DataContext = cloudSyncViewModel;
            PropertyDescriptor pd = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            foreach (DataGridColumn column in dataGrid.Columns)
            {
                //Add a listener for this column's width
                pd.AddValueChanged(column, new EventHandler(ColumnWidthPropertyChanged));
            }
        }
        private bool _isResizeColumnEventFire;
        private bool _columnWidthChanging;
        private bool _handlerAdded;
        private void ColumnWidthPropertyChanged(object sender, EventArgs e)
        {
            // listen for when the mouse is released
            _columnWidthChanging = true;
            if (!_handlerAdded && sender != null)
            {
                _handlerAdded = true;  /* only add this once */
                Mouse.AddPreviewMouseUpHandler(this.dataGrid, BaseDataGrid_MouseLeftButtonUp);
            }
        }
        void BaseDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_columnWidthChanging)
            {
                _isResizeColumnEventFire = true;
                _columnWidthChanging = false;
                // save settings, etc
                SaveColumnWidth();
                _isResizeColumnEventFire = false;
            }

            if (_handlerAdded)  /* remove the handler we added earlier */
            {
                _handlerAdded = false;
                Mouse.RemovePreviewMouseUpHandler(this.dataGrid, BaseDataGrid_MouseLeftButtonUp);
            }
        }
        
        private void SaveColumnWidth()
        {
            DataGridSettings model = vm.DataGridSettingList.Where(x => x.UserId == 0
            && x.ModuleId == (int)ModuleNames.CloudSync
            && x.GridName == DataGridNames.CloudSync.ToString()
            && x.GridConfigName == DataGridConfigName.ColumnWidth.ToString()).FirstOrDefault();

            if (model == null)
            {
                model = new DataGridSettings();
                model = GetDataGridSettingModel(model);
                model.Id = 0;
                model.GridConfigName = DataGridConfigName.ColumnWidth.ToString();
                model.GridConfigValue = "5,25,10,5,5,5,5,7,7,5,5,5,5,5";
            }
            else if (_isResizeColumnEventFire)
            {
                List<string> lst = new List<string>();
                foreach (DataGridColumn column in this.dataGrid.Columns)
                {
                    string val = Math.Floor(Convert.ToDecimal(column.ActualWidth * 100 / this.dataGrid.ActualWidth)).ToString();
                    lst.Add(val);
                }
                model.GridConfigValue = string.Join(",", lst);
            }

            model.InsertOrUpdateDataGridSettings(model);
            var data = new DataGridSettings().GetDataGridSettings();
            if (data != null)
                vm.DataGridSettingList = data.ToObservableCollection();

            string[] values = model.GridConfigValue.Split(',').Select(sValue => sValue.Trim()).ToArray();
            for (int i = 0; i < values.Length - 1; i++)
            {
                dataGrid.Columns[i].Width = (this.dataGrid.ActualWidth * Convert.ToDouble(values[i])) / 100;
            }
        }
        private DataGridSettings GetDataGridSettingModel(DataGridSettings dataGridSettings)
        {
            dataGridSettings.UserId = 0;
            dataGridSettings.ModuleId = (int)ModuleNames.CloudSync;
            dataGridSettings.GridName = DataGridNames.CloudSync.ToString();
            return dataGridSettings;
        }

        private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (cloudSyncViewModel.UpcomingAppointmentList != null && cloudSyncViewModel.UpcomingAppointmentList.Count > 0)
                foreach (var item in cloudSyncViewModel.UpcomingAppointmentList)
                    item.IsSelected = true;
        }

        private void HeaderCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (cloudSyncViewModel.UpcomingAppointmentList != null && cloudSyncViewModel.UpcomingAppointmentList.Count > 0)
                foreach (var item in cloudSyncViewModel.UpcomingAppointmentList)
                    item.IsSelected = false;
        }

        private void IndivisualCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            cloudSyncViewModel.UpcomingAppointmentList.FirstOrDefault(x => x.AppointmentId == (int)checkBox.Tag).IsSelected = true;
        }

        private void IndivisualCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            cloudSyncViewModel.UpcomingAppointmentList.FirstOrDefault(x => x.AppointmentId == (int)checkBox.Tag).IsSelected = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SaveColumnWidth();
        }
    }
}
