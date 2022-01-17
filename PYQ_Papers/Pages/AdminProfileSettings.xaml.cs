using System.Data;
using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.Session;
using static PYQ_Papers.commons;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for AdminProfileSettings.xaml
    /// </summary>
    public partial class AdminProfileSettings : Page
    {
        public AdminProfileSettings()
        {
            InitializeComponent();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new ChangeAdminAuth());
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AddAdminAuth());
        }

        private void DeleteCurrentAdmin_Click(object sender, RoutedEventArgs e)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var dt = new DataTable();
            Set_Command("SELECT * FROM admin");
            _ = da.Fill(dt);

            if (dt.Rows.Count > 1)
            {
                Set_Command("DELETE FROM admin WHERE ID='" + adminId + "'");
                _ = command.ExecuteNonQuery();

                adminId = -1;
                adminName = null;
                isLoggedIn = false;

                _ = MessageBox.Show("Successfully deleted admin");

                _ = NavigationService?.Navigate(new LoginPage());
            }
            else
            {
                _ = MessageBox.Show(
                    "There exists only 1 admin account. Application must contain at least 1 admin. Please try updating credentials for better security.");
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }
    }
}