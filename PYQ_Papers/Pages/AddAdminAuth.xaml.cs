using System.Data;
using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.commons;
using static PYQ_Papers.Session;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for AddAdminAuth.xaml
    /// </summary>
    public partial class AddAdminAuth : Page
    {
        public AddAdminAuth()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AdminProfileSettings());
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(adminUsername.Text) && !string.IsNullOrEmpty(adminPassword.Text))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                Set_Command("INSERT INTO admin (username, password) VALUES('" + adminUsername.Text.Trim() + "' , '" +
                            adminPassword.Text + "')");
                _ = command.ExecuteNonQuery();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                _ = MessageBox.Show("Inserted " + adminUsername.Text.Trim());
                _ = NavigationService?.Navigate(new AdminProfileSettings());
            }
            else
            {
                var parent = Window.GetWindow(this);
                var animation = parent?.Resources["TextBoxAnimation"] as System.Windows.Media.Animation.Storyboard;
                if (string.IsNullOrEmpty(adminUsername.Text))
                {
                    animation?.Begin(adminUsername);
                }

                if (string.IsNullOrEmpty(adminPassword.Text))
                {
                    animation?.Begin(adminPassword);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoggedIn)
                _ = NavigationService?.Navigate(new AdminProfileSettings());
        }
    }
}