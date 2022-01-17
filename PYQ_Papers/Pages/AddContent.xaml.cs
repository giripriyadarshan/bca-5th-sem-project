using System.Windows;
using System.IO;
using Microsoft.Win32;
using Path = System.IO.Path;
using static PYQ_Papers.commons;
using static PYQ_Papers.Session;
using System.Windows.Media.Animation;
using System.Data;

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

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // validate semester input
            if (int.TryParse(SemesterInput.Text.Trim(), out _))
            {
                var sem = new DataTable();
                Set_Command("Select * FROM semester WHERE sem='" + SemesterInput.Text.Trim() + "'");
                _ = da.Fill(sem);

                if (!(sem.Rows.Count > 0))
                {
                    Set_Command("INSERT INTO semester(sem) VALUES(@semester)");
                    command.Parameters.Clear();
                    _ = command.Parameters.AddWithValue("@semester", SemesterInput.Text.Trim());
                    _ = command.ExecuteNonQuery();
                }
            }
            else
            {
                _ = MessageBox.Show("Please input valid semester number in arithmetic digits i.e 6",
                    "INVALID SEMESTER INPUT");
                return;
            }

            // validate subject input
            var sub = new DataTable();
            int subjectID;
            Set_Command("SELECT * FROM subject WHERE subject_name='" +
                        SubjectInput.Text.Trim().ToLower() +
                        "' AND semesterID='" + SemesterInput.Text.Trim() + "'");
            _ = da.Fill(sub);

            if (sub.Rows.Count > 0)
            {
                subjectID = int.Parse(sub.Rows[0][0].ToString());
            }
            else
            {
                Set_Command(
                    "INSERT INTO subject(subject_name, semesterID) OUTPUT INSERTED.ID VALUES(@subject_name, @semesterID)");
                command.Parameters.Clear();
                _ = command.Parameters.AddWithValue("@subject_name", SubjectInput.Text.Trim().ToLower());
                _ = command.Parameters.AddWithValue("@semesterID", SemesterInput.Text.Trim());
                subjectID = int.Parse(command.ExecuteScalar().ToString());
            }

            // validate year input
            if (!int.TryParse(YearInput.Text.Trim(), out _))
            {
                _ = MessageBox.Show("Please enter valid year number in arithmetic digits i.e 2021",
                    "INVALID YEAR INPUT");
                return;
            }

            var year = new DataTable();
            int yearID;
            Set_Command("SELECT * FROM year WHERE year_number='" + YearInput.Text.Trim() + "' AND subjectID='" +
                        subjectID + "'");
            _ = da.Fill(year);

            if (year.Rows.Count > 0)
            {
                yearID = int.Parse(year.Rows[0][0].ToString());
            }
            else
            {
                Set_Command(
                    "INSERT INTO year(year_number, subjectID) OUTPUT INSERTED.ID VALUES(@year_number, @subjectID)");
                command.Parameters.Clear();
                _ = command.Parameters.AddWithValue("@year_number", YearInput.Text.Trim());
                _ = command.Parameters.AddWithValue("@subjectID", subjectID);
                yearID = int.Parse(command.ExecuteScalar().ToString());
            }

            // input for files table
            _ = Directory.CreateDirectory(Set_File_Storage_String(yearID));
            var files = new DataTable();
            Set_Command("SELECT * FROM files WHERE file_name='" + _filename + "' AND yearID='" + yearID + "'");
            _ = da.Fill(files);

            if (files.Rows.Count > 0)
            {
                if (MessageBox.Show(
                        "File already exists in that period \nDo you want to replace the existing file with current file?",
                        "File already exists", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    File.Delete(Set_File_Storage_String(yearID) + _filename);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Set_Command("INSERT INTO files(file_name, yearID) VALUES(@file_name, @yearID)");
                command.Parameters.Clear();
                _ = command.Parameters.AddWithValue("@file_name", _filename);
                _ = command.Parameters.AddWithValue("@yearID", yearID);
                _ = command.ExecuteNonQuery();
            }

            // file copies for both replacement and creation ... so 
            File.Copy(_filepath, Set_File_Storage_String(yearID) + _filename);

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

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

            if (string.IsNullOrWhiteSpace(SemesterInput.Text))
            {
                isInputValid = false;
                animation?.Begin(SemesterInput);
            }

            if (string.IsNullOrWhiteSpace(SubjectInput.Text))
            {
                isInputValid = false;
                animation?.Begin(SubjectInput);
            }

            if (string.IsNullOrWhiteSpace(YearInput.Text))
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