using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
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

        //private HashSet<string> semesterSet = new HashSet<string>();

        /*
        
        For every sem, add the subjects
        For every subject in a sem, add the years
        For every year in a sub, for a sem, add the papers

        Sem
        : primary key - sem numbers
        
        Sub
        : ID
        : subject names
        : foriegn key - sem number
        (No duplications of entire row)
        Method?
            : if select * from sub where subjectName=@subname and semNumber=@semnunmber returns 0 rows, then INSERT values
        and extract the ID of the inserted row
        else
        extract the ID of the existing row
        
        Year
        : ID
        : year numbers
        : subID Foriegn Key
        (No duplications of entire row)
        Method? 
            : if SELECT * FROM year WHERE sem=@semester AND sub=@subject returns 0 rows, then INSERT INTO year (year, sem, sub) VALUES (@year, @sem, @sub)
        and extract the ID of the inserted row
        else
        extract the ID of the existing row

        Files
        : ID
        : Name
        : yearID  

        */

        /*

        //////////////////////////////////////////////////////////
        /// probable method to implement collections
        
        ' select * from sem '

        for each row in sem
            add sem to list (new)
            ' select * from sub where sem number = sem '

            for each row in sub
                add sub to sem's list (new)
                ' select * from year where subID = ID '

                for each row in year
                    add year to sub's list

        */

        /*
                    
        // list all files

        ' select * from files '
        ListOfFiles.Title = row["name"]
        ListOfFiles.yearID = row["yearID"]
        // reason for yearId in the option?
        segregate the file storage in folders named using yearID to avoid confusion among files with similar name

         */

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
                        "subjectID=" + dtrow2["ID"]
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

            connection.Close();
        }

        private void MainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MenuItem selectedItem)) return;

            if (selectedItem.YearID == null) return;

            FileList.Items.Clear();
            var files = new DataTable();
            Set_Command("SELECT * FROM files WHERE yearID='" + selectedItem.YearID + "'");
            connection.Open();
            _ = da.Fill(files);

            foreach (DataRow dtrow in files.Rows)
            {
                _ = FileList.Items.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["filename"].ToString(),
                    YearID = dtrow["yearID"].ToString()
                });
            }

            connection.Close();
        }

        private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // if no item is selected, then it used to open the _fileStorage directory
            if (FileList.SelectedItem == null) return;
            var x = FileList.SelectedItem as ListOfFiles;
            _ = System.Diagnostics.Process.Start(_fileStorage + x?.YearID + x?.PathOfFile);
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
        public static Image AdobePdfIcon = new Image
        {
            Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\" + "\\Images\\" +
                                                 "\\adobepdfimage.png"))
        };
}
}