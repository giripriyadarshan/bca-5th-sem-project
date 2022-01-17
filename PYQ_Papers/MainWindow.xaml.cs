﻿using System.Windows;
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
            Application.Current.Shutdown();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
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
    }
}
