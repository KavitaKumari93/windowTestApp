using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FF.Cockpit.Common;
using Syncfusion.UI.Xaml.Scheduler;

namespace FF.Cockpit.Entity.EntityModel
{
    public class RecurrenceModel : PropertyChangeHelper
    {
        //public RecurrenceModel()
        //{
        //  new RecurrenceModel(); 
        //}
        #region Recurrence property

        private bool _isRecurrence;
        public bool IsRecurrence
        {
            get { return _isRecurrence; }
            set { _isRecurrence = value; if (value) RecurrenceType = "Daily"; else RecurrenceType = ""; OnPropertyChanged(); }
        }

        private string _recurrenceType;
        public string RecurrenceType
        {
            get { return _recurrenceType; }
            set { _recurrenceType = value; OnPropertyChanged(); }
        }

        #region Repeat On

        //repeatOn_DayOfMonth
        private int dayOfMonth;
        public int DayOfMonth
        {
            get { return dayOfMonth; }
            set { dayOfMonth = value; if (value < 0) throw new InvalidDataException("Day 0f month should not be negative value"); }
        }

        //repeatOn_WeekOfMonthCMB
        public int SelectedWeekNumber { get; set; }

        //repeatOn_WeekDayCMB
        public string SelectedWeekDay { get; set; }

        //repeatOn_WeekDaysCMB
        public ObservableCollection<string> SelectedWeekDays { get; set; }

        //repeatOn_MonthCMB
        public int SelectedMonthNumber { get; set; }

        #endregion

        #region Repeat Every
        private int _interval = 1;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; OnPropertyChanged(); }
        }
        #endregion

        #region END 

        private string _selectedEndType = "Never";
        public string SelectedEndType
        {
            get { return _selectedEndType; }
            set
            {
                _selectedEndType = value;
                if (!string.IsNullOrWhiteSpace(value) && value == "Never")
                {
                    EndDate = null;
                    RecurrenceCount = 0;
                    RecurrenceRange = RecurrenceRange.NoEndDate;
                }
                else if (SelectedEndType == "Untill")
                {
                    RecurrenceRange = RecurrenceRange.EndDate;
                }
                else if (SelectedEndType == "Count")
                {
                    RecurrenceRange = RecurrenceRange.Count;
                }
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }

        private int _recurrenceCount;
        public int RecurrenceCount
        {
            get { return _recurrenceCount; }
            set { _recurrenceCount = value; OnPropertyChanged(); }
        }

        private RecurrenceRange _recurrenceRange;
        public RecurrenceRange RecurrenceRange
        {
            get { return _recurrenceRange; }
            set { _recurrenceRange = value; OnPropertyChanged(); }
        }

        #endregion

        #endregion

        #region Methods 

        public ObservableCollection<string> GetRepeatOnList()
        {
            return new ObservableCollection<string>()
            {
                "Daily","Weekly","Monthly","Yearly"
            };
        }
        public ObservableCollection<string> GetEndList()
        {
            return new ObservableCollection<string>()
            {
                "Never","Until","Count"
            };
        }
        public ObservableCollection<string> GetDayOfWeekList()
        {
            return new ObservableCollection<string>()
            {
                "Sunday","Monday","Tuesday","Wednesday","Thursday","Friday", "Saturday"
            };
        }
        public Dictionary<int, string> GetWeekOfMonthList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "First");
            dic.Add(2, "Second");
            dic.Add(3, "Third");
            dic.Add(4, "Fourth");
            dic.Add(-1, "Last");

            return dic;
        }
        public Dictionary<int, string> GetMonthOfYearList()
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            dic.Add(1, "January");
            dic.Add(2, "February");
            dic.Add(3, "March");
            dic.Add(4, "April");
            dic.Add(5, "May");
            dic.Add(6, "June");
            dic.Add(7, "July");
            dic.Add(8, "August");
            dic.Add(9, "September");
            dic.Add(10, "October");
            dic.Add(11, "November");
            dic.Add(12, "December");
            return dic;
        }
        #endregion
    }
}

