using FF.Cockpit.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class MedicalHistoryTileData_Result : PropertyChangeHelper
    {
        public MedicalHistoryTileData_Result()
        {
            FillMedicalHistoryComboValue();
        }

        #region Properties
        private ObservableCollection<MedicalHistorycomboItems> medicalHistorycomboItemsObj;
        public ObservableCollection<MedicalHistorycomboItems> MedicalHistorycomboItemsObj
        {
            get { return medicalHistorycomboItemsObj; }
            set { medicalHistorycomboItemsObj = value; OnPropertyChanged(); }
        }
        private int? patientId;
        public int? PatientId
        {
            get { return patientId; }
            set { patientId = value; OnPropertyChanged(); }
        }
        private Visibility lastUpdatedVisible;
        public Visibility LastUpdatedVisible
        {
            get { return lastUpdatedVisible; }
            set { lastUpdatedVisible = value; OnPropertyChanged(); }
        }
        private bool isSkinCancerAnalysed;
        public bool IsSkinCancerAnalysed
        {
            get { return isSkinCancerAnalysed; }
            set { isSkinCancerAnalysed = value; OnPropertyChanged(); }
        }
        private bool isCancerAnalysed;
        public bool IsCancerAnalysed
        {
            get { return isCancerAnalysed; }
            set { isCancerAnalysed = value; OnPropertyChanged(); }
        }
        private bool isGeneticalConspicuousAnalsyed;
        public bool IsGeneticalConspicuousAnalsyed
        {
            get { return isGeneticalConspicuousAnalsyed; }
            set { isGeneticalConspicuousAnalsyed = value; OnPropertyChanged(); }
        }
        private MedicalHistorycomboItems selectedskinCancerOption;
        public MedicalHistorycomboItems SelectedskinCancerOption
        {
            get { return selectedskinCancerOption; }
            set { selectedskinCancerOption = value; OnPropertyChanged(); }
        }
        private MedicalHistorycomboItems selectedCancerOption;
        public MedicalHistorycomboItems SelectedCancerOption
        {
            get { return selectedCancerOption; }
            set { selectedCancerOption = value; OnPropertyChanged(); }
        }
        private MedicalHistorycomboItems selectedgeneticalConspicuousOption;
        public MedicalHistorycomboItems SelectedgeneticalConspicuousOption
        {
            get { return selectedgeneticalConspicuousOption; }
            set { selectedgeneticalConspicuousOption = value; OnPropertyChanged(); }
        }
        private string lastUpdatedOn;
        public string LastUpdatedOn
        {
            get { return lastUpdatedOn; }
            set { lastUpdatedOn = value; OnPropertyChanged(); }
        }
        
        #endregion



        #region Methods
        public async Task<MedicalHistoryTileData_Result> GetMedicalHistoryByPatientId(int? patientId)
        {
            try
            {
                using (var repo = new GenericRepository<MedicalHistoryTileData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetPatientMedicalHistoryOperationsById, new { @patientId = patientId});
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public async Task<long> InsertMedicalHistoryByPatientId(MedicalHistoryTileData_Result historyTileModel)
        {
            try
            {
                using (var repo = new GenericRepository<MedicalHistoryTileData_Result>())
                {
                    await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_InsertPatientMedicalHistoryOperationsById,
                           new
                           {
                               @patientId = historyTileModel.PatientId,
                               @skinCancerAnalysed = historyTileModel.IsSkinCancerAnalysed,
                               @cancerAnalysed = historyTileModel.IsCancerAnalysed,
                               @geneticalConspicuousAnalysed = historyTileModel.IsGeneticalConspicuousAnalsyed
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
      
        public void FillMedicalHistoryComboValue()
        {
            MedicalHistorycomboItemsObj = new ObservableCollection<MedicalHistorycomboItems>();
            MedicalHistorycomboItemsObj.Add(new MedicalHistorycomboItems() { selectedId = 0, DisplayName = "No" });
            MedicalHistorycomboItemsObj.Add(new MedicalHistorycomboItems() { selectedId = 1, DisplayName = "Yes" });
        }
        public MedicalHistorycomboItems GetSelectedComboboxItem(bool selectedId)
        {
            MedicalHistorycomboItems medicalHistorycomboItems = null;
           if (MedicalHistorycomboItemsObj!=null)
            {
                medicalHistorycomboItems = MedicalHistorycomboItemsObj.FirstOrDefault(x => x.selectedId == Convert.ToInt32(selectedId));
            }
           return medicalHistorycomboItems;
        }


        #endregion
    }
    public class MedicalHistorycomboItems
    {
        public int selectedId { get; set; }
        public string DisplayName { get; set; }
    }
}
