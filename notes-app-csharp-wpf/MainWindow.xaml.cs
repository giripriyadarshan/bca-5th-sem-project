using System.Windows;

namespace notes_app_csharp_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.Contents());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.SlideshowOrHomepage());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.LoginPage());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
