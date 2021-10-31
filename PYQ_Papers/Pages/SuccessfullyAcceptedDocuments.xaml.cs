using System.Windows;
using static PYQ_Papers.commons;

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
            adminLoginSession = false;
            _ = NavigationService?.Navigate(new Contents());
        }

        private void uploadagain_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AddContent());
        }
    }
}