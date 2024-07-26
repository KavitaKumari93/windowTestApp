using FF.Cockpit.Common;
using FF.Cockpit.Entity.EntityModel;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Xml.Linq;
using localisation=FF.Cockpit.Common.Properties.Resources;

namespace FF.Cockpit.UI.Helpers
{
    public static class AppointmentRefData
    {
        public static ObservableCollection<MiscellaneousSettingsModel> miscellaneousData = null;
        public static Dictionary<double, string> durationData = null;
        public static Dictionary<int, string> recurrenceTypeData = null;
        static AppointmentRefData()
        {
            miscellaneousData = new ObservableCollection<MiscellaneousSettingsModel>();
            durationData = new Dictionary<double, string>();
            recurrenceTypeData = new Dictionary<int, string>();

            miscellaneousData = GetMiscellaneousList();
            durationData = GetDurationList();
            recurrenceTypeData = GetAppointmentRecurrenceType();
        }


        private static ObservableCollection<MiscellaneousSettingsModel> GetMiscellaneousList()
        {
            ObservableCollection<MiscellaneousSettingsModel> lst = new ObservableCollection<MiscellaneousSettingsModel>();

            var data = new MiscellaneousSettingsModel().GetMiscellaneousData_sync();
            if (data != null && data.Count() > 0)
                lst = data.ToObservableCollection();

            foreach (var name in Enum.GetNames(typeof(MiscellaneousKeys)))
            {
                if (!lst.Any(x => x.Name == name.ToString()))
                    lst.Add(new MiscellaneousSettingsModel() { Name = name.ToString() });
            }
            return lst;
        }

        public static Dictionary<double, string> GetStartTimeList(double duration = 30)
        {
            return SrartAndEndTime(duration);
        }
        public static Dictionary<double, string> GetEndTimeList(double duration = 30)
        {
            return SrartAndEndTime(duration, true);
        }
        private static Dictionary<double, string> SrartAndEndTime(double duration, bool isEndTimeList = false)
        {
            double fromTime = 00;//00:00
            double toTime = 1440;//24:00

            if (!isEndTimeList)
            {
                var startHourObj = miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.StartHour.ToString())?.Value;
                var endHourObj = miscellaneousData.FirstOrDefault(x => x.Name == MiscellaneousKeys.EndHour.ToString())?.Value;

                fromTime = startHourObj != null ? Convert.ToDouble(startHourObj) : 360;
                toTime = endHourObj != null ? Convert.ToDouble(endHourObj) : 1200;

                DurationTypes durationValue;
                Enum.TryParse<DurationTypes>(duration.ToString(), out durationValue);

                switch (durationValue)
                {
                    case DurationTypes.ThirtyMinutes:
                        toTime = toTime - (double)DurationTypes.ThirtyMinutes;
                        break;
                    case DurationTypes.SixtyMinutes:
                        toTime = toTime - (double)DurationTypes.SixtyMinutes;
                        break;
                    case DurationTypes.NinetyMinutes:
                        toTime = toTime - (double)DurationTypes.NinetyMinutes;
                        break;
                    case DurationTypes.OneTwentyMinutes:
                        toTime = toTime - (double)DurationTypes.OneTwentyMinutes;
                        break;
                }
            }

            return Extension.GetTimeWithMinutes(fromTime, toTime);
        }
        private static Dictionary<double, string> GetDurationList()
        {
            Dictionary<double, string> dic = new Dictionary<double, string>();
            dic.Add(30, "30 mins");
            dic.Add(60, "1 hour");
            dic.Add(90, "1h 30 mins");
            dic.Add(120, "2 hours");
            //dic.Add(150, "2h 30 mins");
            //dic.Add(180, "3 hours");
            //dic.Add(210, "3h 30 mins");
            //dic.Add(240, "4 hours");
            //dic.Add(270, "4h 30 mins");
            //dic.Add(300, "5 hours");
            //dic.Add(330, "5h 30 mins");
            //dic.Add(360, "6 hours");
            //dic.Add(390, "6h 30 mins");
            //dic.Add(420, "7 hours");
            //dic.Add(450, "7h 30 mins");
            //dic.Add(480, "8 hours");
            //dic.Add(510, "8h 30 mins");
            //dic.Add(540, "9 hours");
            //dic.Add(570, "9h 30 mins");
            //dic.Add(600, "10 hours");
            //dic.Add(630, "10h 30 mins");
            //dic.Add(660, "11 hours");
            //dic.Add(690, "11h 30 mins");
            //dic.Add(720, "12 hours");
            //dic.Add(750, "12h 30 mins");
            //dic.Add(780, "13 hours");
            //dic.Add(810, "13h 30 mins");
            //dic.Add(840, "14 hours");
            //dic.Add(870, "14h 30 mins");
            //dic.Add(900, "15 hours");
            //dic.Add(930, "15h 30 mins");
            //dic.Add(960, "16 hours");
            //dic.Add(990, "16h 30 mins");
            //dic.Add(1020, "17 hours");
            //dic.Add(1050, "17h 30 mins");
            //dic.Add(1080, "18 hours");
            //dic.Add(1110, "18h 30 mins");
            //dic.Add(1140, "19 hours");
            //dic.Add(1170, "19h 30 mins");
            //dic.Add(1200, "20 hours");
            //dic.Add(1230, "20h 30 mins");
            //dic.Add(1260, "21 hours");
            //dic.Add(1290, "21h 30 mins");
            //dic.Add(1320, "22 hours");
            //dic.Add(1350, "22h 30 mins");
            //dic.Add(1380, "23 hours");
            //dic.Add(1410, "23h 30 mins");
            //dic.Add(1440, "24 hours");
            return dic;
        }
        private static Dictionary<int, string> GetAppointmentRecurrenceType()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(3, $"3 {localisation.RecurrenceMonth_resx}");
            dic.Add(6, $"6 {localisation.RecurrenceMonth_resx}");
            dic.Add(12, $"{localisation.RecurrenceYear_resx}");
            return dic;
        }

    }
}
