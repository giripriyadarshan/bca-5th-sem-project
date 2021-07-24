using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_button_OnClick(object sender, RoutedEventArgs e)
        {
            connection.Open();
            var dt = new DataTable();
            Set_Command("SELECT * FROM users WHERE username='" + UsernameInput.Text.Trim() +
                        "' AND password='" + PasswordInput.Password.ToString() + "'");
            _ = da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                adminLoginSession = true;
                _ = NavigationService.Navigate(new AddContent());
            }
            else
            {
                _ = NavigationService.Navigate(new Contents());
            }

            connection.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (adminLoginSession)
            {
                _ = NavigationService.Navigate(new AddContent());
            }
        }
    }
}