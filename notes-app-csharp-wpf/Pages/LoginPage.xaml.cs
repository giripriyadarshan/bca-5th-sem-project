using System.Data;
using System.Windows;
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
                        "' AND password='" + PasswordInput.Password + "'");
            _ = da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                adminLoginSession = true;
                _ = NavigationService?.Navigate(new AddContent());
            }
            else
            {
                _ = NavigationService?.Navigate(new Contents());
            }

            connection.Close();
        }
 
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (adminLoginSession)
            {
                _ = NavigationService?.Navigate(new AddContent());
            }
        }
    }
}