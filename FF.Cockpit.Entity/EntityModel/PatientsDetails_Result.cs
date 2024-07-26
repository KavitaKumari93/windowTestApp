using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class Patient : PropertyChangeHelper
    {
        #region Constructor
        public Patient()
        {

        }
        #endregion

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
        private string gender;
        public string Gender
        {
            get { return gender; }
            set { gender = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods 
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            try
            {
                using (var repo = new GenericRepository<Patient>())
                {
                    return await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetPatients, null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        public IEnumerable<Patient> SyncPatientDumpData()
        {
            try
            {
                using (var repo = new GenericRepository<Patient>())
                {
                    return repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.SP_MergeMasterTableData, null);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        #endregion
    }
}
