using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF.Cockpit.Entity.EntityModel
{
    public class UserLoginModel:PropertyChangeHelper
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; OnPropertyChanged(); }
        }
    }
}
