using PYQ_Papers.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using static PYQ_Papers.commons;

namespace PYQ_Papers.Pages
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
            var context = new Context();

            var semester = new MenuItem() { Title = "Semester", YearID = -1 };

            foreach (var sems in context.Semesters.OrderBy(sem => sem.Sem).ToList())
            {
                var subject = new MenuItem() { Title = sems.Sem.ToString(), YearID = -1 };

                foreach (var subs in context.Subjects
                    .Where(sub => sub.Semester.Sem == sems.Sem)
                    .OrderBy(sub => sub.SubjectName)
                    .ToList())
                {
                    var year = new MenuItem() { Title = subs.SubjectName, YearID = -1 };

                    foreach (var years in context.Years
                        .Where(y => y.Subject.Id == subs.Id)
                        .OrderBy(y => y.YearNumber)
                        .ToList())
                    {
                        year.Items.Add(new MenuItem()
                        {
                            Title = years.YearNumber.ToString(),
                            YearID = years.Id
                        });
                    }

                    subject.Items.Add(year);
                }
                semester.Items.Add(subject);
            }

            _ = MainList.Items.Add(semester);

            Add_All_Files(ref FileList);
        }

        private void MainList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MenuItem selectedItem)) return;

            if (selectedItem.YearID < 1) return;

            list.Clear();
            FileList.ItemsSource = null;
            FileList.Items.Clear();

            var context = new Context();

            foreach (var file in context.Files.Where(f => f.Year.Id == selectedItem.YearID).Include(f => f.Year).ToList())
            {
                list.Add(new ListOfFiles()
                {
                    PathOfFile = file.FileName,
                    YearID = file.Year.Id,
                });

                FileList.ItemsSource = list;
            }
        }

        private void FileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // if no item is selected, then it used to open the _fileStorage directory
            if (FileList.SelectedItem == null) return;
            var x = FileList.SelectedItem as ListOfFiles;

            var context = new Context();
            var file = context.Files.Where(f => f.FileName == x.PathOfFile)
                .Include(f => f.Year)
                .Where(f => f.Year.Id == x.YearID)
                .Single();
            file.NumberOfTimesOpened++;
            context.SaveChanges();

            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(x.YearID) + x.PathOfFile);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FileList.ItemsSource = null;
            FileList.Items.Clear();
            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                foreach (ListOfFiles i in list)
                {
                    string content = ReadFile(Set_File_Storage_String(int.Parse(i.YearID.ToString())) + i.PathOfFile);

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
        public int YearID { get; set; }
    }

    public class ListOfFiles
    {
        public string PathOfFile { get; set; }
        public int YearID { get; set; }
        public int OpenedCount { get; set; }
    }
}
