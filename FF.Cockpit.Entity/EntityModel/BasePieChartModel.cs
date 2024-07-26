using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class BasePieChartModel
    {
        #region Property
        public string Title { get; set; }
        public double Value { get; set; }
        public string Color { get; set; }
        #endregion

        #region Constructor
        public BasePieChartModel()
        {

        }
        #endregion

    }
}
