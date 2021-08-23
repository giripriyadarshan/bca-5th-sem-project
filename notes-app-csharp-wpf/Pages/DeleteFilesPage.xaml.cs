using System.IO;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static notes_app_csharp_wpf.commons;
using System.Windows.Controls.Primitives;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for DeleteFilesPage.xaml
    /// </summary>
    public partial class DeleteFilesPage : Page
    {
        public DeleteFilesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            var sem = new MenuItem() { Title = "Semester" };

            var dt = new DataTable();
            Set_Command("SELECT * FROM semester ORDER BY sem ASC");
            _ = da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                var sub = new MenuItem()
                {
                    Title = dtrow["sem"].ToString(),
                };


                Set_Command("SELECT * FROM subject WHERE semesterID='" + dtrow["sem"] + "' ORDER BY subject_name ASC");
                var s = new DataTable();
                _ = da.Fill(s);

                foreach (DataRow dtrow2 in s.Rows)
                {
                    var year = new MenuItem()
                    {
                        Title = dtrow2["subject_name"].ToString(),
                    };

                    Set_Command(
                        "SELECT * FROM year WHERE " +
                        "subjectID='" + dtrow2["ID"] + "' ORDER BY year_number ASC"
                    );

                    var y = new DataTable();
                    _ = da.Fill(y);

                    foreach (DataRow dtrow3 in y.Rows)
                    {
                        year.Items.Add(new MenuItem()
                        {
                            Title = dtrow3["year_number"].ToString(),
                            YearID = dtrow3["ID"].ToString()
                        });
                    }

                    sub.Items.Add(year);
                }

                sem.Items.Add(sub);
            }

            _ = MainList.Items.Add(sem);

            // load all files in the beginning
            Add_All_Files(ref FileList);

            connection.Close();
        }

        private void MainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MenuItem selectedItem)) return;

            if (selectedItem.YearID == null) return;

            FileList.ItemsSource = null;
            FileList.Items.Clear();
            var files = new DataTable();
            Set_Command("SELECT * FROM files WHERE yearID='" + selectedItem.YearID + "'");

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            _ = da.Fill(files);

            foreach (DataRow dtrow in files.Rows)
            {
                _ = FileList.Items.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["file_name"].ToString(),
                    YearID = dtrow["yearID"].ToString(),
                });
            }

            connection.Close();
        }

        private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DeleteButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileList.SelectedItem == null) return;

            var x = FileList.SelectedItem as ListOfFiles;
            const string deletefilestring = "Delete the file ";
            if (MessageBox.Show("are you sure ?",
                deletefilestring + x?.PathOfFile, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                File.Delete(Set_File_Storage_String(int.Parse(x.YearID)) + x.PathOfFile);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                Set_Command("DELETE FROM files WHERE file_name='" + x.PathOfFile + "' AND yearID='" + x.YearID + "'");
                command.ExecuteNonQuery();

                // check if other files in the same yearID exists
                var file = new DataTable();
                Set_Command("SELECT * FROM files WHERE yearID='" + x.YearID + "'");
                _ = da.Fill(file);

                if (file.Rows.Count < 1)
                {
                    var year = new DataTable();
                    int subjectID;
                    Set_Command("SELECT * FROM year WHERE ID='" + x.YearID + "'");
                    _ = da.Fill(year);

                    subjectID = int.Parse(year.Rows[0][2].ToString());

                    Set_Command("DELETE FROM year WHERE ID='" + x.YearID + "'");
                    _ = command.ExecuteNonQuery();

                    var sub1 = new DataTable();
                    Set_Command("SELECT * FROM year WHERE subjectID='" + subjectID + "'");
                    _ = da.Fill(sub1);

                    if (sub1.Rows.Count < 1)
                    {
                        var sub2 = new DataTable();
                        int semesterID;
                        Set_Command("SELECT * FROM subject WHERE ID='" + subjectID + "'");
                        _ = da.Fill(sub2);

                        semesterID = int.Parse(sub2.Rows[0][2].ToString());

                        Set_Command("DELETE FROM subject WHERE ID='" + subjectID + "'");
                        _ = command.ExecuteNonQuery();

                        var semester = new DataTable();
                        Set_Command("SELECT * FROM subject WHERE semesterID='" + semesterID + "'");
                        _ = da.Fill(semester);

                        if (semester.Rows.Count < 1)
                        {
                            Set_Command("DELETE FROM semester WHERE sem='" + semesterID + "'");
                            _ = command.ExecuteNonQuery();
                        }
                    }
                }
            }


            _ = NavigationService?.Navigate(new DeleteFilesPage());
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FileList.ItemsSource = null;
            FileList.Items.Clear();
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                foreach (ListOfFiles i in list)
                {
                    string content = ReadFile(Set_File_Storage_String(int.Parse(i.YearID)) + i.PathOfFile);

                    if (content.ToLower().Contains(SearchBox.Text.ToLower()))
                    {
                        _ = FileList.Items.Add(i);
                    }
                }
            }
            else
            {
                Add_All_Files(ref FileList);
            }
        }
    }
}