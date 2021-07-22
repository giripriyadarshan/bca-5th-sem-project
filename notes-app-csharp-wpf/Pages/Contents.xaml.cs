using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
            connection.Open();

            var sem = new MenuItem() {Title = "Semester"};

            var dt = new DataTable();
            Set_Command("SELECT * FROM semester");
            _ = da.Fill(dt);

            foreach (DataRow dtrow in dt.Rows)
            {
                var sub = new MenuItem()
                {
                    Title = dtrow["semester"].ToString(),
                };

                Set_Command("SELECT * FROM subject WHERE semesterID=" + dtrow["Id"]);
                var s = new DataTable();
                _ = da.Fill(s);

                foreach (DataRow dtrow2 in s.Rows)
                {
                    var year = new MenuItem()
                    {
                        Title = dtrow2["subject_name"].ToString(),
                    };

                    Set_Command(
                        "SELECT * FROM year WHERE semesterID=" +
                        dtrow2["semesterID"] +
                        " AND subjectID=" + dtrow2["Id"]
                    );
                    var y = new DataTable();
                    _ = da.Fill(y);

                    foreach (DataRow dtrow3 in y.Rows)
                    {
                        year.Items.Add(new MenuItem()
                        {
                            Title = dtrow3["year"].ToString() + dtrow3["filename"],
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
            if (!(e.NewValue is MenuItem selectedItem)) return;
            // create a template to display file-names using listbox
            // double click on the item of the list will open th e pdf file associated with the name
            // to open a file use
            // System.Diagnostics.Process.Start(@"|DataDirectory|\\Resources\\files\\" + selectedItem.Index);
            FileList.Items.Clear();
            var files = new DataTable();
            Set_Command("SELECT * FROM year WHERE Id='" + selectedItem.Index + "'");
            connection.Open();
            _ = da.Fill(files);
            var x = new Image
            {
                Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + "\\Images\\" +
                                                 "\\adobepdfimage.png"))
            };

            foreach (DataRow dtrow in files.Rows)
            {
                FileList.Items.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["filename"].ToString(),
                    AdobePdfIcon = x
                });
            }

            connection.Close();
        }

        private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // just to show how to access the filename displayed on the list
            var x = FileList.SelectedItem as ListOfFiles;
            MessageBox.Show(x?.PathOfFile);
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

    public class ListOfFiles
    {
        public string PathOfFile { get; set; }
        public Image AdobePdfIcon { get; set; }
    }
}