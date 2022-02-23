using PYQ_Papers.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static PYQ_Papers.commons;

namespace PYQ_Papers.Pages
{
    /// <summary>
    /// Interaction logic for ContentDetails.xaml
    /// </summary>
    public partial class ContentDetails : Page
    {
        public ContentDetails()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Files(ref Ascending, true);
            Files(ref Descending, false);
        }

        private void Files(ref ListBox fileList, bool isAscending)
        {
            var context = new Context();
            fileList.Items.Clear();

            var files = new List<File>();


            if (isAscending)
                files = context.Files.OrderBy(f => f.NumberOfTimesOpened).Take(10).ToList();
            else
                files = context.Files.OrderByDescending(f => f.NumberOfTimesOpened).Take(10).ToList();

            foreach (var file in files)
            {
                fileList.Items.Add(new ListOfFiles()
                {
                    PathOfFile = file.FileName,
                    OpenedCount = file.NumberOfTimesOpened,
                    YearID = file.Year.Id
                });
            }

        }

        private void Descending_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Descending.SelectedItem == null) return;

            var x = Descending.SelectedItem as ListOfFiles;

            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(x.YearID) + x.PathOfFile);
        }

        private void Ascending_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Ascending.SelectedItem == null) return;

            var x = Ascending.SelectedItem as ListOfFiles;

            _ = System.Diagnostics.Process.Start(Set_File_Storage_String(x.YearID) + x.PathOfFile);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService?.Navigate(new PostLoginMenuPage());
        }
    }
}
