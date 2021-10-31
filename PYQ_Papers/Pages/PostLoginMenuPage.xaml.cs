using System.Windows;
using System.Windows.Controls;

using static PYQ_Papers.commons;

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
            adminLoginSession = false;
            _ = NavigationService?.Navigate(new LoginPage());
        }

        private void DeletePapers_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new DeleteFilesPage());
        }
    }
}