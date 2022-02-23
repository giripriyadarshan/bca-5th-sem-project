using PYQ_Papers.Models;
using System.Windows;
using System.Windows.Controls;
using static PYQ_Papers.Session;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for ChangeAdminAuth.xaml
    /// </summary>
    public partial class ChangeAdminAuth : Page
    {
        public ChangeAdminAuth()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new AdminProfileSettings());
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(adminUsername.Text) && !string.IsNullOrEmpty(adminPassword.Text))
            {
                var context = new Context();
                var admin = context.Admins.Find(adminId);
                admin.Username = adminUsername.Text.Trim();
                admin.Password = adminPassword.Text;
                context.SaveChanges();

                adminName = adminUsername.Text;
                _ = MessageBox.Show("Updated");
                _ = NavigationService?.Navigate(new AdminProfileSettings());

            }
            else
            {
                var parent = Window.GetWindow(this);
                var animation = parent?.Resources["TextBoxAnimation"] as System.Windows.Media.Animation.Storyboard;
                if (string.IsNullOrEmpty(adminUsername.Text))
                {
                    animation?.Begin(adminUsername);
                }

                if (string.IsNullOrEmpty(adminPassword.Text))
                {
                    animation?.Begin(adminPassword);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoggedIn)
                _ = NavigationService?.Navigate(new AdminProfileSettings());
            else
            {
                adminUsername.Text = adminName;
            }
        }
    }
}