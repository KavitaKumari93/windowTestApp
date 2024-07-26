using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using FF.Cockpit.Common;
using FF.Cockpit.Entity.StoreProcedure_Result;
using Syncfusion.UI.Xaml.Scheduler;

namespace FF.Cockpit.Entity.EntityModel
{
    public class AppointmentDurationModel : PropertyChangeHelper
    {
        #region Property

        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set { _duration = value; if (value != null && value > 0) DurationStr = value + " minutes"; OnPropertyChanged(); }
        }

        private string _durationStr;
        public string DurationStr
        {
            get { return _durationStr; }
            set { _durationStr = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods 
        #endregion
    }

}

