using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for Contents.xaml
    /// </summary>
    public partial class Contents
    {
        public Contents()
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
            // if no item is selected, then it used to open the _fileStorage directory
            if (FileList.SelectedItem == null) return;
            var x = FileList.SelectedItem as ListOfFiles;
            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(int.Parse(x.YearID)) + x.PathOfFile);
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

        private void SearchBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }

    public class MenuItem
    {
        public ObservableCollection<MenuItem> Items { get; set; }

        public MenuItem()
        {
            Items = new ObservableCollection<MenuItem>();
        }

        public string Title { get; set; }
        public string YearID { get; set; }
    }

    public class ListOfFiles
    {
        public string PathOfFile { get; set; }
        public string YearID { get; set; }
    }
}