using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public List<ListOfFiles> list = new List<ListOfFiles>();

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

            Add_All_Files();

            connection.Close();
        }

        private void MainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MenuItem selectedItem)) return;

            if (selectedItem.YearID == null) return;

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

        private void Add_All_Files()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // load all files in the beginning
            var allfiles = new DataTable();
            Set_Command("SELECT * FROM files");
            _ = da.Fill(allfiles);


            list.Clear();
            FileList.Items.Clear();
            foreach (DataRow dtrow in allfiles.Rows)
            {
                list.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["file_name"].ToString(),
                    YearID = dtrow["yearID"].ToString(),

                });
            }

            FileList.ItemsSource = list;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private string ReadFile(string pdfPath)
        {
            var pageText = new StringBuilder();
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfPath)))
            {
                var pageNumbers = pdfDocument.GetNumberOfPages();
                for (int i = 1; i <= pageNumbers; i++)
                {
                    LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                    parser.ProcessPageContent(pdfDocument.GetFirstPage());
                    pageText.Append(strategy.GetResultantText());
                }
            }
            return pageText.ToString();
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
                Add_All_Files();
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

        // change index to sem and year seperately
        // the purpose is to call the "year" query filtering duplicates and store the sem and sub to list all the files in that year
        public string YearID { get; set; }
    }

    public class ListOfFiles
    {
        public string PathOfFile { get; set; }
        public string YearID { get; set; }
        public string ImagePath { get; } = @"E:\dotnet-practice\notes-app-csharp-wpf\notes-app-csharp-wpf\bin\Debug\Resources\Images\adobepdfimage.png";
    }
}
