using FF.Cockpit.Common;
using System;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class DashboardTilesData_Result : PropertyChangeHelper
    {
        public DashboardTilesData_Result()
        {
        }

        #region Property
        private string patientId;
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; OnPropertyChanged(); }
        }

        private string patientName;
        public string PatientName
        {
            get { return patientName; }
            set { patientName = value; OnPropertyChanged(); }
        }
        private string recordNumber;
        public string RecordNumber
        {
            get { return recordNumber; }
            set { recordNumber = value; OnPropertyChanged(); }
        }

        private string dOB;
        public string DOB
        {
            get { return dOB; }
            set { dOB = value; OnPropertyChanged(); }
        }

        private int skinTypeId;
        public int SkinTypeId
        {
            get { return skinTypeId; }
            set { skinTypeId = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; if (SkinTypeId == 0) description = "Unselected"; OnPropertyChanged(); }
        }

        private int sessionCount;
        public int SessionCount
        {
            get { return sessionCount; }
            set { sessionCount = value; OnPropertyChanged(); }
        }

        private int microImagesCount;
        public int MicroImagesCount
        {
            get { return microImagesCount; }
            set { microImagesCount = value; OnPropertyChanged(); }
        }

        private double excisionCount;
        public double ExcisionCount
        {
            get { return excisionCount; }
            set { excisionCount = value; OnPropertyChanged(); }
        }

        private double lesionsCount;
        public double LesionsCount
        {
            get { return lesionsCount; }
            set { lesionsCount = value; OnPropertyChanged(); }
        }
        private string examinationTime;
        public string ExaminationTime
        {
            get { return examinationTime; }
            set { examinationTime = value; OnPropertyChanged(); }
        }

        private DateTime? _appointmentStartDate;
        public DateTime? AppointmentStartDate
        {
            get { return _appointmentStartDate; }
            set { _appointmentStartDate = value; OnPropertyChanged(); }
        }
        private DateTime? _appointmentEndDate;
        public DateTime? AppointmentEndDate
        {
            get { return _appointmentEndDate; }
            set { _appointmentEndDate = value; OnPropertyChanged(); }
        }

        private string _nextAppointmentDate;
        public string NextAppointmentDate
        {
            get { return _nextAppointmentDate; }
            set { _nextAppointmentDate = value; OnPropertyChanged(); }
        }
        private int _lesionsFollowUpCount;
        public int LesionsFollowUpCount
        {
            get { return _lesionsFollowUpCount; }
            set { _lesionsFollowUpCount = value; OnPropertyChanged(); }
        }
        private int _ghostMarkersCount;
        public int GhostMarkersCount
        {
            get { return _ghostMarkersCount; }
            set { _ghostMarkersCount = value; OnPropertyChanged(); }
        }
        private int _lesionsUnderFollowUpCount;
        public int LesionsUnderFollowUpCount
        {
            get { return _lesionsUnderFollowUpCount; }
            set { _lesionsUnderFollowUpCount = value; OnPropertyChanged(); }
        }
        private int _deactivatedMarkers;
        public int DeactivatedMarkers
        {
            get { return _deactivatedMarkers; }
            set { _deactivatedMarkers = value; OnPropertyChanged(); }
        }

        #endregion

        public async Task<DashboardTilesData_Result> GetDashboardTilesData(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<DashboardTilesData_Result>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_GetDashboardTilesDataByPatientId, new { @patientObjectID = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public DashboardTilesData_Result GetDashboardTilesData_WithoutSync(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<DashboardTilesData_Result>())
                {
                    return  repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_GetDashboardTilesDataByPatientId, new { @patientObjectID = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public async Task<int> UpdateSkinTileData(int? patientId, int skinTypeId)
        {
            try
            {
                using (var repo = new GenericRepository<int>())
                {
                    return await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_UpdateDashboardSkinTypeByPatientId, new { @PatientId = patientId, @SkinTypeId = skinTypeId });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return 0;
            }
        }

    }
}
