using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.StoreProcedure_Result
{
    public class PerformanceLowFrequentPatientsData_Result : PropertyChangeHelper
    {
        public PerformanceLowFrequentPatientsData_Result()
        {

        }
        #region Property
        private int? objectId;
        public int? ObjectId
        {
            get { return objectId; }
            set { objectId = value; OnPropertyChanged(); }
        }
        private string patientRecordnumber;
        public string PatientRecordnumber
        {
            get { return patientRecordnumber; }
            set { patientRecordnumber = value; OnPropertyChanged(); }
        }
        private string concatPatientRecordnumber;
        public string ConcatpatientRecordnumber
        {
            get { return $"ID:{this.PatientRecordnumber}"; }
            set { concatPatientRecordnumber = value; OnPropertyChanged(); }
        }
        private string patientName;
        public string PatientName
        {
            //get { return this.FirstName[0] + "." + ", " + this.LastName[0]; }
            get { return this.FirstName + " " + this.MiddleName + " " + this.LastName; }
            set { patientName = value; OnPropertyChanged(); }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; OnPropertyChanged(); }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; OnPropertyChanged(); }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; OnPropertyChanged(); }
        }
        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; OnPropertyChanged(); }
        }
        private string lastUpdate;
        public string LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; OnPropertyChanged(); }
        }
        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; OnPropertyChanged(); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(); }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }
        #endregion


        public async Task<IEnumerable<PerformanceLowFrequentPatientsData_Result>> GetLowFrequentPatients(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (var repo = new GenericRepository<PerformanceLowFrequentPatientsData_Result>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetLowFrequentPatients, new { @FromDate = fromDate, @ToDate = toDate });

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public  IEnumerable<PerformanceLowFrequentPatientsData_Result> GetLowFrequentPatient(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (var repo = new GenericRepository<PerformanceLowFrequentPatientsData_Result>())
                {
                    return  repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetLowFrequentPatients, new { @FromDate = fromDate, @ToDate = toDate });

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
