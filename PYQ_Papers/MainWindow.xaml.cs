using System.Windows;
using static PYQ_Papers.Session;

namespace PYQ_Papers
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
            _ = MainFrame.NavigationService.Navigate(new Pages.SlideshowOrHomepage());
        }

        private void Home_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.SlideshowOrHomepage());
        }

        private void Login_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.LoginPage());
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.AboutPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            adminId = -1;
            adminName = null;
            isLoggedIn = false;
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                _ = MainFrame.RemoveBackEntry();
            }
        }

        private void Content_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _ = MainFrame.NavigationService.Navigate(new Pages.Contents());
        }
    }
}
