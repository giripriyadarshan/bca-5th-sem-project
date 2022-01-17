using System.Windows;
using static PYQ_Papers.Session;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for SuccessfullyAcceptedDocuments.xaml
    /// </summary>
    public partial class SuccessfullyAcceptedDocuments
    {
        public SuccessfullyAcceptedDocuments()
        {
            InitializeComponent();
        }

        private void contents_Click(object sender, RoutedEventArgs e)
        {
            adminId = -1;
            adminName = null;
            isLoggedIn = false;
            _ = NavigationService?.Navigate(new Contents());
        }

        private void uploadagain_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AddContent());
        }
    }
}