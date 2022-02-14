using System.Windows;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for SlideshowOrHomepage.xaml
    /// </summary>
    public partial class SlideshowOrHomepage
    {
        public SlideshowOrHomepage()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Papers_Click(object sender, RoutedEventArgs e)
        {
            _ = (NavigationService?.Navigate(new Contents()));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["MainBackground"] = Application.Current.Resources["SecondaryBackground"];
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["SecondaryBackground"] = Application.Current.Resources["MainBackground"];
        }
    }
}