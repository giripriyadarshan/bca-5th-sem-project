using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for SuccessfullyAcceptedDocuments.xaml
    /// </summary>
    public partial class SuccessfullyAcceptedDocuments : Page
    {
        public SuccessfullyAcceptedDocuments()
        {
            InitializeComponent();
        }

        private void contents_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.Navigate(new Contents());
        }

        private void uploadagain_Click(object sender, RoutedEventArgs e)
        {
            adminLoginSession = true;
            _ = NavigationService.Navigate(new AddContent());
        }
    }
}
