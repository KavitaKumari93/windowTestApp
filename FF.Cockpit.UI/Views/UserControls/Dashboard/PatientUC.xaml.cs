using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace FF.Cockpit.UI.Views.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for PatientUC.xaml
    /// </summary>
    public partial class PatientUC : UserControl
    {
        public PatientUC()
        {
            InitializeComponent();
            TbxSearchPatientWatermark.Visibility = Visibility.Visible;
        }

        private void TbxSearchPatient_GotFocus(object sender, RoutedEventArgs e)
        {
               TbxSearchPatientWatermark.Visibility = Visibility.Collapsed;
                autoCompletorListPopup.Width = TbxSearchPatient.ActualWidth;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TbxSearchPatient.Text.Length  == 0)
                TbxSearchPatientWatermark.Visibility = Visibility.Visible;
            else
                TbxSearchPatientWatermark.Visibility = Visibility.Collapsed;
        }
    }
}
