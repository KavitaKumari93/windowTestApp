using FF.Cockpit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF.Cockpit.UI.Views.UserControls.Performance
{
    /// <summary>
    /// Interaction logic for PerformanceFilterTile.xaml
    /// </summary>
    public partial class PerformanceFilterTile : UserControl
    {
        public PerformanceFilterTile()
        {
            InitializeComponent();
             this.Calendar.Language= XmlLanguage.GetLanguage(LanguageHelper.LanguageName);
        }

        private void calendarPopup_Opened(object sender, EventArgs e)
        {
           AppStarter.IsPerformanceCalendarPopUpOpen = calendarPopup;
        }
    }
}
