using FF.Cockpit.Common;
using FF.Cockpit.UI.Helpers;
using FF.Cockpit.UI.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FF.Cockpit.UI.ViewModels
{
    public class LeftBottomMenuViewModel : PropertyChangeHelper
    {
        public ICommand BackCommand { get; private set; }
        public LeftBottomMenuViewModel()
        {
            BackCommand = new RelayCommandHelper(x => OnBack(x));
        }
        private void OnBack(object letfBottomMenu)
        {
            try
            {
                MainViewModel vm = (MainViewModel)AppStarter.mainVM;
                vm.BindSelectedView();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
    }
}
