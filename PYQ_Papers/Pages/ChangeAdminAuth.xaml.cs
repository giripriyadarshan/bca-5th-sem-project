using System.Data;
using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.Session;
using static PYQ_Papers.commons;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for ChangeAdminAuth.xaml
    /// </summary>
    public partial class ChangeAdminAuth : Page
    {
        public ChangeAdminAuth()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AdminProfileSettings());
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(adminUsername.Text) && !string.IsNullOrEmpty(adminPassword.Text))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                Set_Command("UPDATE admin SET username='" + adminUsername.Text.Trim() + "', password='" +
                            adminPassword.Text.Trim() + "' WHERE ID='" + adminId + "'");
                _ = command.ExecuteNonQuery();

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }

                adminName = adminUsername.Text;
                _ = MessageBox.Show("Updated");
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
            else
            {
                adminUsername.Text = adminName;
            }
        }
    }
}