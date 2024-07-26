using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FF.Cockpit.Common
{
    public abstract class PropertyChangeHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            try
            {
               
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError(ex);
            }
        }
    }
}
