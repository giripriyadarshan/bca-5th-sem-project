using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static PYQ_Papers.commons;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for ContentDetails.xaml
    /// </summary>
    public partial class ContentDetails : Page
    {
        public ContentDetails()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Files(ref Ascending, true);
            Files(ref Descending, false);
        }

        private void Files(ref ListBox fileList, bool isAscending)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var files = new DataTable();
            var commandString = "SELECT * FROM files ORDER BY no_of_times_opened " + (isAscending ? "ASC " : "DESC ");
            Set_Command(commandString);
            _ = da.Fill(files);
            fileList.Items.Clear();

            foreach (DataRow dtrow in files.Rows)
            {
                fileList.Items.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["file_name"].ToString(),
                    YearID = dtrow["yearID"].ToString(),
                    OpenedCount = dtrow["no_of_times_opened"].ToString(),
                });
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void Descending_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Descending.SelectedItem == null) return;

            var x = Descending.SelectedItem as ListOfFiles;

            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(int.Parse(x.YearID)) + x.PathOfFile);
        }

        private void Ascending_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Ascending.SelectedItem == null) return;

            var x = Ascending.SelectedItem as ListOfFiles;

            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(int.Parse(x.YearID)) + x.PathOfFile);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }
    }
}
