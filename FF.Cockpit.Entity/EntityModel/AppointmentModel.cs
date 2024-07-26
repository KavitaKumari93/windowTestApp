using FF.Cockpit.Common;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class AppointmentModel : ScheduleAppointment, INotifyPropertyChanged
    {
        public AppointmentModel() { }

        #region Property

        private bool _isFullSeriesAppointmentEdit;
        public bool IsFullSeriesAppointmentEdit
        {
            get { return _isFullSeriesAppointmentEdit; }
            set { _isFullSeriesAppointmentEdit = value; OnPropertyChanged(); }
        }

        private int? _appointmentTypeId;
        public int? AppointmentTypeId
        {
            get { return _appointmentTypeId; }
            set { _appointmentTypeId = value; OnPropertyChanged(); }
        }

        private string _appointmentTypeName;
        public string AppointmentTypeName
        {
            get { return _appointmentTypeName; }
            set { _appointmentTypeName = value; OnPropertyChanged(); }
        }

        private int? _roomId;
        public int? RoomId
        {
            get { return _roomId; }
            set { _resourceId = _roomId = value; OnPropertyChanged(); }
        }
        private string _roomName;
        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; OnPropertyChanged(); }
        }

        private int? _resourceId;
        public int? ResourceId
        {
            get { return _resourceId; }
            set { _resourceId = value; OnPropertyChanged(); }
        }
        private int? _doctorId;
        public int? DoctorId
        {
            get { return _doctorId; }
            set { _doctorId = value; OnPropertyChanged(); }
        }

        private string _doctorName;
        public string DoctorName
        {
            get { return _doctorName; }
            set { _doctorName = value; OnPropertyChanged(); }
        }

        private int? _patientId;
        public int? PatientId
        {
            get { return _patientId; }
            set { _patientId = value; OnPropertyChanged(); }
        }
        private string _patientName;
        public string PatientName
        {
            get { return _patientName; }
            set { _patientName = value; OnPropertyChanged(); }
        }
        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; OnPropertyChanged(); }
        }

        private bool _isRecurrence;
        public bool IsRecurrence
        {
            get { return _isRecurrence; }
            set { _isFullSeriesAppointmentEdit = _isRecurrence = value; OnPropertyChanged(); }
        }

        private int _recurrenceMonth;
        public int RecurrenceMonth
        {
            get { return _recurrenceMonth; }
            set { _recurrenceMonth = value; OnPropertyChanged(); }
        }

        private int _recursiveId;
        public int RecursiveId
        {
            get { return _recursiveId; }
            set { _recursiveId = value; OnPropertyChanged(); }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnPropertyChanged(); }
        }

        private string _viewType;
        public string ViewType
        {
            get { return _viewType; }
            set { _viewType = value; OnPropertyChanged(); }
        }
        private ObservableCollection<object> _resources;
        public ObservableCollection<object> Resources
        {
            get { return _resources; }
            set { _resources = value; OnPropertyChanged(); }
        }

        private string _startTimeStr;
        public string StartTimeStr
        {
            get { return _startTimeStr; }
            set { _startTimeStr = value; OnPropertyChanged(); }
        }

        private string _endTimeStr;
        public string EndTimeStr
        {
            get { return _endTimeStr; }
            set { _endTimeStr = value; OnPropertyChanged(); }
        }

        private double _duration;
        public double Duration
        {
            get { return _duration; }
            set { _duration = value; OnPropertyChanged(); }
        }
        #endregion

        #region Methods 
        public async Task<IEnumerable<Patient>> GetPatientsInformation()
        {
            try
            {
                using (var repo = new GenericRepository<Patient>())
                {
                    var data = await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetPatients, new { });
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        public async Task<int> InsertOrUpdateAppointment(AppointmentModel appointment)
        {
            int id = 0;
            try
            {
                Task.Delay(1500);
                using (var repo = new GenericRepository<AppointmentModel>())
                {
                    var model = await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_InsertOrUpdateAppointment,
                          new
                          {
                              @id = appointment.Id,
                              @notes = appointment.Notes,
                              @startTime = appointment.StartTime.ToUniversalTime(),
                              @endTime = appointment.EndTime.ToUniversalTime(),


                              @patientId = appointment.PatientId,
                              @appointmentTypeId = appointment.AppointmentTypeId,
                              @roomId = appointment.RoomId,
                              @doctorId = appointment.DoctorId,
                              @isRecurrence = appointment.IsRecurrence,
                              @recurrenceMonth = appointment.RecurrenceMonth,
                              @recursiveId = appointment.RecursiveId,
                              @viewType = appointment.ViewType
                          });

                    Task.Delay(1000);
                    id = Convert.ToInt32(model.Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

            return id;
        }

        public int InsertOrUpdateAppointment_sync(AppointmentModel appointment)
        {
            int id = 0;
            try
            {
                using (var repo = new GenericRepository<AppointmentModel>())
                {
                    var model = repo.ExcuteProcedureWithSingleResult_sync(DatabaseHelper.sp_InsertOrUpdateAppointment,
                          new
                          {
                              @id = appointment.Id,
                              //@subject = appointment.Subject,
                              @notes = appointment.Notes,
                              @startTime = appointment.StartTime.ToUniversalTime(),
                              @endTime = appointment.EndTime.ToUniversalTime(),

                              @appointmentTypeId = appointment.AppointmentTypeId,
                              @viewType = appointment.ViewType,
                              @roomId = appointment.RoomId,
                              @doctorId = appointment.DoctorId,
                              @patientId = appointment.PatientId,
                              @isRecurrence = appointment.IsRecurrence,
                              @recurrenceMonth = appointment.RecurrenceMonth,
                              @recursiveId = appointment.RecursiveId
                          });

                    id = Convert.ToInt32(model.Id);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

            return id;
        }
        public async Task<bool> DeleteAppointment(int appointmentId, int recursiveId, bool isFullSeriesDelete)
        {
            bool isDeleted = false;
            try
            {
                using (var repo = new GenericRepository<AppointmentModel>())
                {

                    var model = await repo.ExcuteProcedureWithSingleResult(DatabaseHelper.sp_DeleteAppointment,
                          new
                          {
                              @id = appointmentId,
                              @recursiveId = recursiveId,
                              @isFullSeriesDelete = isFullSeriesDelete
                          });

                    Task.Delay(1000);
                    isDeleted = Convert.ToBoolean(model.Id);
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

            return isDeleted;
        }

        public async Task<IEnumerable<AppointmentModel>> GetAppointmentList(DateTime fromDate, DateTime toDate, int? patientId, int? roomId, int? doctorId)
        {
            try
            {
                using (var repo = new GenericRepository<AppointmentModel>())
                {
                    var data = await repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetAppointments,
                        new
                        {
                            fromDate,
                            toDate,
                            patientId,
                            roomId,
                            doctorId
                        });
                    Task.Delay(5000);
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }

        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion
    }
}

