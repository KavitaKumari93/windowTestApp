using System.Windows;

namespace FF.Cockpit.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Window
    {
        public AddRoom(object vm)
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