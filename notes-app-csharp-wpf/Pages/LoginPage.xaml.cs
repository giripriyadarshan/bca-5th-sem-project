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
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var dt = new DataTable();
            Set_Command("SELECT * FROM admin WHERE username='" + UsernameInput.Text.Trim() + "'");
            _ = da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][1].ToString() == PasswordInput.Password)
                {
                    adminLoginSession = true;
                    _ = NavigationService?.Navigate(new PostLoginMenuPage());
                }
                else
                {
                    _ = MessageBox.Show("Invalid Password");
                }
            }
            else
            {
                _ = MessageBox.Show("Invalid Username");
            }

            connection.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (adminLoginSession)
            {
                _ = NavigationService?.Navigate(new PostLoginMenuPage());
            }
        }
    }
}