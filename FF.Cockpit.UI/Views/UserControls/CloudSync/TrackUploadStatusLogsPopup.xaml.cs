using FF.Cockpit.UI.ViewModels;
using System.Data;
using System.Windows;
using System.Collections.Generic;
using FF.Cockpit.Entity.StoreProcedure_Result;
using System.Reflection;
using System.Linq;
using FF.Cockpit.Common;
using System.Threading;

namespace FF.Cockpit.UI.Views.UserControls.CloudSync
{
    /// <summary>
    /// Interaction logic for TrackUploadStatusLogsPopup.xaml
    /// </summary>
    public partial class TrackUploadStatusLogsPopup : Window
    {
        private readonly CloudSyncViewModel _cloudSyncViewModel = null;
        public TrackUploadStatusLogsPopup(CloudSyncViewModel cloudSyncViewModel)
        {
            _cloudSyncViewModel = cloudSyncViewModel;
            InitializeComponent();
            this.DataContext = _cloudSyncViewModel;
        }
    }
}
