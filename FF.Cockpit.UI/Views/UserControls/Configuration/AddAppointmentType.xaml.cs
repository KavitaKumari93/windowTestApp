using System;
using System.Windows;

namespace FF.Cockpit.UI.Views.UserControls.Configuration
{
    /// <summary>
    /// Interaction logic for AddAppointmentType.xaml
    /// </summary>
    public partial class AddAppointmentType : Window
    {
        public AddAppointmentType(Object vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
