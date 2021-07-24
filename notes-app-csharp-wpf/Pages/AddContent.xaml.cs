using System.Windows;
using System.IO;
using Microsoft.Win32;
using Path = System.IO.Path;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
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

        // Directory.CreateDirectory(path);
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

            // to see what we get as filename
            // it appears that the entire file path is shown as filename
            // so using Path.GetFileName we can extract filename only
            if (!pdfFile.CheckPathExists) return;
            _pathExists = true;
            _filepath = pdfFile.FileName;
            _filename = Path.GetFileName(_filepath);
            _ = MessageBox.Show(_filename);

        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_pathExists) return;
            _ = Directory.CreateDirectory(_fileStorage);

            connection.Open();

            Set_Command("INSERT INTO subject(subject_name, semesterID) OUTPUT INSERTED.Id VALUES(@subject, @semester)");
            _ = command.Parameters.AddWithValue("@subject", SubjectInput.Text.Trim());
            _ = command.Parameters.AddWithValue("@semester", SemesterInput.Text.Trim());

            var subjectId = command.ExecuteScalar();

            Set_Command("INSERT INTO year(year, subjectID, semesterID, filename) " +
                        "VALUES(@year, @subjectID, @semesterID, @filename)");
            _ = command.Parameters.AddWithValue("@year", YearInput.Text.Trim());
            _ = command.Parameters.AddWithValue("@subjectID", subjectId.ToString());
            _ = command.Parameters.AddWithValue("@semesterID", SemesterInput.Text.Trim());
            _ = command.Parameters.AddWithValue("@filename", _filename);

            _ = command.ExecuteNonQuery();

            File.Copy(_filepath , _fileStorage + _filename);

            connection.Close();
            _pathExists = false;
            _ = NavigationService.Navigate(new SuccessfullyAcceptedDocuments());
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            adminLoginSession = false;
            _ = NavigationService.Navigate(new LoginPage());
        }
    }
}
