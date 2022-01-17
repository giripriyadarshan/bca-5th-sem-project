using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.Session;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for PostLoginMenuPage.xaml
    /// </summary>
    public partial class PostLoginMenuPage : Page
    {
        public PostLoginMenuPage()
        {
            InitializeComponent();
        }

        private void AddPapers_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AddContent());
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            adminId = -1;
            adminName = null;
            isLoggedIn = false;
            _ = NavigationService?.Navigate(new LoginPage());
        }

        private void DeletePapers_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new DeleteFilesPage());
        }

        private void AdminSettings_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AdminProfileSettings());
        }
    }
}