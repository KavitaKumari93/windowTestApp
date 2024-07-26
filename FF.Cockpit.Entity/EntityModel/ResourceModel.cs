using FF.Cockpit.Common;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
//using FF.Cockpit.Common.Properties;

namespace FF.Cockpit.Entity.EntityModel
{
    public class ResourceModel : PropertyChangeHelper
    {
        private int _resourceId;
        public int ResourceId
        {
            get { return _resourceId; }
            set { _resourceId = value; OnPropertyChanged(); }
        }

        private string _resourceName;
        public string ResourceName
        {
            get { return _resourceName; }
            set { _resourceName = value; OnPropertyChanged(); }
        }

        private Brush _resourceBgColourBrush;
        public Brush ResourceBgColourBrush
        {
            get { return _resourceBgColourBrush; }
            set { _resourceBgColourBrush = value; OnPropertyChanged(); }
        }


        private string _appointmentBgColourCode;
        public string AppointmentBgColourCode
        {
            get { return _appointmentBgColourCode; }
            set { _appointmentBgColourCode = value; OnPropertyChanged(); }
        }

        private bool _isRoomResource;
        public bool IsRoomResource
        {
            get { return _isRoomResource; }
            set { _isRoomResource = value; OnPropertyChanged(); }
        }


        private double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; OnPropertyChanged(); }
        }
        public async Task<ICollection<ResourceModel>> GetResourcesList(object schedulerType)
        {
            IList<ResourceModel> resources = new List<ResourceModel>();
            try
            {
                SchedulerTypes schedulerTypes = (SchedulerTypes)schedulerType;

                switch (schedulerTypes)
                {
                    case SchedulerTypes.Room:
                        using (var repo = new GenericRepository<RoomModel>())
                        {
                            var data = repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetRooms, new { });
                            if (data != null && data.Result.Count() > 0)
                            {
                                foreach (var item in data.Result)
                                {
                                    resources.Add(new ResourceModel()
                                    {
                                        ResourceId = item.RoomId,
                                        ResourceName = item.RoomName,
                                        AppointmentBgColourCode = item.LightThemeColor,
                                        IsRoomResource = true
                                    });
                                }
                            }
                        }
                        break;
                    case SchedulerTypes.Doctor:
                        using (var repo = new GenericRepository<UsersInformationModel>())
                        {
                            var data = repo.ExcuteProcedureWithMultiResult(DatabaseHelper.sp_GetUsersInformation, new { });
                            if (data != null && data.Result.Count() > 0)
                            {
                                foreach (var item in data.Result)
                                {
                                    resources.Add(new ResourceModel()
                                    {
                                        ResourceId = item.UserId,
                                        ResourceName = item.UserName,
                                        AppointmentBgColourCode = item.LightThemeColor,
                                        IsRoomResource = false
                                    });
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }

            return await Task.FromResult(resources);
        }
        public IEnumerable<ResourceModel> GetResourcesList_sync(object schedulerType)
        {
            ICollection<ResourceModel> resources = new List<ResourceModel>();

            try
            {
                SchedulerTypes schedulerTypes = (SchedulerTypes)schedulerType;

                switch (schedulerTypes)
                {
                    case SchedulerTypes.Room:
                        using (var repo = new GenericRepository<RoomModel>())
                        {
                            IEnumerable<RoomModel> list = repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetRooms, new { });
                            foreach (var item in list)
                            {
                                resources.Add(new ResourceModel()
                                {
                                    ResourceId = item.RoomId,
                                    ResourceName = item.RoomName,
                                    AppointmentBgColourCode = item.LightThemeColor,
                                    IsRoomResource = true
                                });
                            }
                        }
                        break;
                    case SchedulerTypes.Doctor:
                        using (var repo = new GenericRepository<UsersInformationModel>())
                        {
                            IEnumerable<UsersInformationModel> list = repo.ExcuteProcedureWithMultiResult_sync(DatabaseHelper.sp_GetUsersInformation, new { });
                            foreach (var item in list)
                            {
                                resources.Add(new ResourceModel()
                                {
                                    ResourceId = item.UserId,
                                    ResourceName = item.UserName,
                                    AppointmentBgColourCode = item.LightThemeColor,
                                    IsRoomResource = false
                                });
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
            return resources;
        }
    }

    enum SchedulerTypes
    {
        Room = 1,
        Doctor = 2
    }
}
