using PYQ_Papers.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.Session;

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
            var context = new Context();
            if (context.Admins.Count() > 1)
            {
                context.Admins.Remove(context.Admins.Where(a => a.Id == adminId).Single());
                context.SaveChanges();

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
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }
    }
}