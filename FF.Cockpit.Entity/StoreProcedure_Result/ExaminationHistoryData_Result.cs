using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class ExaminationHistoryData_Result : PropertyChangeHelper
    {
        public ExaminationHistoryData_Result()
        {
        }

        #region Property

        private DateTime sessionDate;
        public DateTime SessionDate
        {
            get { return sessionDate; }
            set { if (sessionDate != null) { sessionDateString = value.ToString(ConstantHelper.DateFormat); sessionDate = value; } OnPropertyChanged(); }
        }

        private int bodyScanCount;
        public int BodyScanCount
        {
            get { return bodyScanCount; }
            set { bodyScanCount = value; OnPropertyChanged(); }
        }

        private bool isBodyScanCountChange;
        public bool IsBodyScanCountChange
        {
            get { return isBodyScanCountChange; }
            set { isBodyScanCountChange = value; OnPropertyChanged(); }
        }

        private int markerCount;
        public int MarkerCount
        {
            get { return markerCount; }
            set { markerCount = value; OnPropertyChanged(); }
        }
        private bool isMarkerCountChange;
        public bool IsMarkerCountChange
        {
            get { return isMarkerCountChange; }
            set { isMarkerCountChange = value; OnPropertyChanged(); }
        }
        private int microImageCount;
        public int MicroImageCount
        {
            get { return microImageCount; }
            set { microImageCount = value; OnPropertyChanged(); }
        }
        private bool isMicroImageCountChange;
        public bool IsMicroImageCountChange
        {
            get { return isMicroImageCountChange; }
            set { isMicroImageCountChange = value; OnPropertyChanged(); }
        }
        private int excisionCount;
        public int ExcisionCount
        {
            get { return excisionCount; }
            set { excisionCount = value; OnPropertyChanged(); }
        }
        private bool isExcisionCountChange;
        public bool IsExcisionCountChange
        {
            get { return isExcisionCountChange; }
            set { isExcisionCountChange = value; OnPropertyChanged(); }
        }
        private string comment;
        public string Comment
        {
            get { return comment; }
            set { comment = value; OnPropertyChanged(); }
        }


        private string sessionNumber;
        public string SessionNumber
        {
            get { return sessionNumber; }
            set { sessionNumber = value; OnPropertyChanged(); }
        }

        private string sessionDateString;
        public string SessionDateString
        {
            get { return sessionDateString; }
            set { sessionDateString = value; OnPropertyChanged(); }
        }

        private int tbmImageCount;
        public int TBMImageCount
        {
            get { return tbmImageCount; }
            set { tbmImageCount = value; }
        }

        #endregion

        public async Task<IEnumerable<ExaminationHistoryData_Result>> GetExaminationHistoryTileData(int? patientId, DateTime? selectedFilterDate)
        {
            try
            {
                using (var repo = new GenericRepository<ExaminationHistoryData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetExaminationHistoryDataByPatientId, new { @patientId = patientId, @selectedFilterDate = selectedFilterDate });
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
    }
}
