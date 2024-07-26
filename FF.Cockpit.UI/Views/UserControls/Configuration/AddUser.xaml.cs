using System.Windows;

namespace FF.Cockpit.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser(object vm)
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