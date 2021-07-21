using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for Contents.xaml
    /// </summary>
    public partial class Contents : Page
    {
        public Contents()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();

            MenuItem sem = new MenuItem() { Title = "Semester" };

            DataTable dt = new DataTable();
            Set_Command("SELECT * FROM semester");
            _ = da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {

                MenuItem sub = new MenuItem()
                {
                    Title = dtrow["semester"].ToString(),
                };

                Set_Command("SELECT * FROM subject WHERE semesterID=" + dtrow["Id"].ToString());
                DataTable s = new DataTable();
                _ = da.Fill(s);

                foreach (DataRow dtrow2 in s.Rows)
                {
                    MenuItem year = new MenuItem()
                    {
                        Title = dtrow2["subject_name"].ToString(),
                    };

                    Set_Command(
                        "SELECT * FROM year WHERE semesterID=" +
                        dtrow2["semesterID"].ToString() +
                        " AND subjectID=" + dtrow2["Id"].ToString()
                        );
                    DataTable y = new DataTable();
                    _ = da.Fill(y);

                    foreach (DataRow dtrow3 in y.Rows)
                    {
                        year.Items.Add(new MenuItem()
                        {
                            Title = dtrow3["year"].ToString(),
                            Index = dtrow3["Id"].ToString()
                        });
                    }

                    sub.Items.Add(year);

                }

                sem.Items.Add(sub);

            }

            _ = MainList.Items.Add(sem);

            connection.Close();
        }

        private void MainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MenuItem selectedItem = e.NewValue as MenuItem;
            if (selectedItem != null)
            {
                // create a template to display file-names using listbox
                // double click on the item of the list will open th e pdf file associated with the name
                // to open a file use
                // System.Diagnostics.Process.Start(@"|DataDirectory|\\Resources\\files\\" + selectedItem.Index);
                FileList.Items.Clear();
                DataTable files = new DataTable();
                Set_Command("SELECT * FROM year WHERE Id='" + selectedItem.Index + "'");
                connection.Open();
                _ = da.Fill(files);

                foreach (DataRow dtrow in files.Rows)
                {
                    _ = FileList.Items.Add(dtrow["filename"].ToString());
                }
                connection.Close();
            }
        }

        private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // just to show how to access the filename displayed on the list
            _ = MessageBox.Show(FileList.SelectedItem.ToString());
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

        public string Index { get; set; }
    }
}
