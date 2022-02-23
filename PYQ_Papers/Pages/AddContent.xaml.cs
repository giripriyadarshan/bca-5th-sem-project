using Microsoft.Win32;
using PYQ_Papers.Models;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using static PYQ_Papers.commons;
using static PYQ_Papers.Session;
using Path = System.IO.Path;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for AddContent.xaml
    /// </summary>
    public partial class AddContent
    {
        private string _filename;
        private string _filepath;
        private bool _pathExists;

        public AddContent()
        {
            InitializeComponent();
        }

        private void UploadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pdfFile = new OpenFileDialog
            {
                Title = "Upload the paper",
                RestoreDirectory = true,
                Multiselect = false
            };

            // return if not true
            if (pdfFile.ShowDialog() != true) return;

            if (!pdfFile.CheckPathExists) return;
            _pathExists = true;
            _filepath = pdfFile.FileName;
            _filename = Path.GetFileName(_filepath);
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckForInvalidInput()) return;

            var context = new Context();
            var semester = new Semester();
            var subject = new Subject();
            var year = new Year();
            var file = new File();

            //if (connection.State == ConnectionState.Closed)
            //{
            //    connection.Open();
            //}

            //// validate semester input
            if (int.TryParse(SemesterInput.Text.Trim(), out int semInt))
            {
                if (!context.Semesters.Where(sem => sem.Sem.ToString() == SemesterInput.Text.Trim()).Any())
                {
                    context.Semesters.Add(new Semester() { Sem = semInt });
                    context.SaveChanges();
                }
                semester = context.Semesters.Find(semInt);
            }
            else
            {
                _ = MessageBox.Show("Please input valid semester number in arithmetic digits i.e 6",
                    "INVALID SEMESTER INPUT");
                return;
            }

            // validate subject input
            if (!context.Subjects
                .Where(sub => sub.SubjectName == SubjectInput.Text.Trim().ToLower())
                .Where(sub => sub.Semester == semester)
                .Any())
            {
                context.Subjects.Add(new Subject()
                {
                    SubjectName = SubjectInput.Text.Trim().ToLower(),
                    Semester = semester
                });
                context.SaveChanges();
            }

            subject = context.Subjects.Where(s => s.SubjectName == SubjectInput.Text.Trim().ToLower()).Where(s => s.Semester == semester).Single();

            // validate year input
            if (int.TryParse(YearInput.Text.Trim(), out int yearInt))
            {
                if (!context.Years.Where(y => y.YearNumber == yearInt).Where(y => y.Subject == subject).Any())
                {
                    context.Years.Add(new Year()
                    {
                        YearNumber = yearInt,
                        Subject = subject,
                    });
                    context.SaveChanges();
                }

                year = context.Years.Where(y => y.YearNumber == yearInt).Where(y => y.Subject == subject).Single();
            }
            else
            {
                _ = MessageBox.Show("Please enter valid year number in arithmetic digits i.e 2021",
                    "INVALID YEAR INPUT");
                return;
            }

            // input for files table
            _ = System.IO.Directory.CreateDirectory(Set_File_Storage_String(year.Id));

            file = context.Files.Where(f => f.FileName == _filename).Where(f => f.Year == year).Single();
            if (file == null)
            {
                context.Files.Add(new File()
                {
                    FileName = _filename,
                    Year = year,
                    NumberOfTimesOpened = 0
                });
                context.SaveChanges();
                file = context.Files.Where(f => f.FileName == _filename).Where(f => f.Year == year).Single();
            }
            else
            {
                if (MessageBox.Show(
                        "File already exists in that period \nDo you want to replace the existing file with current file?",
                        "File already exists", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    System.IO.File.Delete(Set_File_Storage_String(file.Year.Id) + _filename);
                    file.NumberOfTimesOpened = 0;
                }
                else
                {
                    return;
                }
            }

            // file copies for both replacement and creation ... so 
            System.IO.File.Copy(_filepath, Set_File_Storage_String(file.Year.Id) + _filename);

            _pathExists = false;
            _ = NavigationService?.Navigate(new SuccessfullyAcceptedDocuments());
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoggedIn)
            {
                _ = NavigationService?.Navigate(new LoginPage());
            }
        }

        private bool CheckForInvalidInput()
        {
            var parent = Window.GetWindow(this);
            var animation = parent?.Resources["TextBoxAnimation"] as Storyboard;
            var isInputValid = true;

            if (string.IsNullOrWhiteSpace(SubjectInput.Text))
            {
                isInputValid = false;
                animation?.Begin(SubjectInput);
            }

            if (YearInput.Text.Length != 4)
            {
                isInputValid = false;
                animation?.Begin(YearInput);
            }

            if (!_pathExists)
            {
                isInputValid = false;
                animation?.Begin(UploadButton);
            }

            return isInputValid;
        }
    }
}