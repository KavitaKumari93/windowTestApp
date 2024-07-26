using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using FF.Cockpit.Entity.StoreProcedure_Result;
using FF.Cockpit.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using static Syncfusion.Windows.Controls.SfNavigator;

namespace FF.Cockpit.UI.Views.UserControls.Performance
{

    /// <summary>
    /// Interaction logic for HighRiskPatientTile.xaml
    /// </summary>
    public partial class HighRiskPatientTile : System.Windows.Controls.UserControl
    {
        MainViewModel vm = null;
        public HighRiskPatientTile()
        {
            InitializeComponent();
            vm = AppStarter.mainVM as MainViewModel;
            PropertyDescriptor pd = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            foreach (DataGridColumn column in dataGrid.Columns)
            {
                //Add a listener for this column's width
                pd.AddValueChanged(column, new EventHandler(ColumnWidthPropertyChanged));
            }
        }


        #region Event fire when we resize the column width of Grid
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
        #endregion

        #region When if page is load then set width of column
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SaveColumnWidth();
        }
        #endregion

        private void SaveColumnWidth()
        {
            DataGridSettings model = vm.DataGridSettingList.Where(x => x.UserId == 0
            && x.ModuleId == (int)ModuleNames.Performance
            && x.GridName == DataGridNames.HighRiskPatient.ToString()
            && x.GridConfigName == DataGridConfigName.ColumnWidth.ToString()).FirstOrDefault();

            if (model == null)
            {
                model = new DataGridSettings();
                model = GetDataGridSettingModel(model);
                model.Id = 0;
                model.GridConfigName = DataGridConfigName.ColumnWidth.ToString();
                model.GridConfigValue = "14,14,10,12,10,10,16,10";
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
            dataGridSettings.ModuleId = (int)ModuleNames.Performance;
            dataGridSettings.GridName = DataGridNames.HighRiskPatient.ToString();
            return dataGridSettings;
        }

        private void DataGridColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader header = sender as DataGridColumnHeader;
            PerformanceViewModel performanceViewModel = (PerformanceViewModel)vm.CurrentView;
            performanceViewModel.Sorting(header);
        }
    }
}
