using FF.Cockpit.UI.ViewModels;
using System.Data;
using System.Windows;
using System.Collections.Generic;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System.Reflection;
using System.Linq;
using FF.Cockpit.Common;
using System.Threading;

namespace FF.Cockpit.UI.Views.UserControls.Performance
{
    /// <summary>
    /// Interaction logic for LowFrequentPatients.xaml
    /// </summary>
    public partial class LowFrequentPatients : Window
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private readonly PerformanceViewModel _performanceViewModel = null;
        public LowFrequentPatients(PerformanceViewModel performanceViewModel)
        {
            _performanceViewModel = performanceViewModel;
            InitializeComponent();
            this.DataContext = performanceViewModel;
            msgSuccess.IsVisibleChanged += MsgSuccess_IsVisibleChanged;
        }

        private void MsgSuccess_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            timer.Tick += Timer_Tick; ;
            timer.Interval = 2000;
            timer.Start();
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (msgSuccess.IsVisible)
            {
                msgSuccess.Visibility = Visibility.Collapsed;
                timer.Stop();
            }
        }

        private async void Export_CSV_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = Extension.GetFilePath("LowFrequentPatients",false);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                msgInProgress.Visibility = Visibility.Visible;
                msgSuccess.Visibility = Visibility.Collapsed;
                var list = new List<PerformanceLowFrequentPatientsData_Result>(patientListGrid.ItemsSource as IEnumerable<PerformanceLowFrequentPatientsData_Result>);
                var dataTable = ToDataTable(list);
                if (dataTable != null)
                {
                    Thread.Sleep(3000);
                    await Extension.ExportToCSV(dataTable, filePath);
                }
                msgInProgress.Visibility = Visibility.Collapsed;
                msgSuccess.Visibility = Visibility.Visible;
            }
        }

        private async void Export_PDF_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string filePath = Extension.GetFilePath("LowFrequentPatients", true);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                msgInProgress.Visibility = Visibility.Visible;
                msgSuccess.Visibility = Visibility.Collapsed;

                var list = new List<PerformanceLowFrequentPatientsData_Result>(patientListGrid.ItemsSource as IEnumerable<PerformanceLowFrequentPatientsData_Result>);
                var dataTable = ToDataTable(list);
                if (dataTable != null)
                {
                    Thread.Sleep(3000);
                    await Extension.ExportToPDF(dataTable, filePath, "Low Frequent Patients");
                }
                msgInProgress.Visibility = Visibility.Collapsed;
                msgSuccess.Visibility = Visibility.Visible;
            }
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Props = Props.Where(p => p.Name != "ObjectId"
            && p.Name != "PatientRecordnumber"
            && p.Name != "ConcatpatientRecordnumber"
            && p.Name != "PatientName"
            //&& p.Name != "FirstName"
            //&& p.Name != "MiddleName"
            //&& p.Name != "MiddleName"
            && p.Name != "DateOfBirth").ToArray();

            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        
    }
}
