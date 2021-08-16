using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static notes_app_csharp_wpf.commons;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for SlideshowOrHomepage.xaml
    /// </summary>

    public partial class SlideshowOrHomepage
    {
        

        public SlideshowOrHomepage()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _pictureTimer.Interval = new TimeSpan(0, 0, 2);
            _pictureTimer.Tick += PictureTimer_Tick;
            _pictureTimer.Start();

            DirectoryInfo dirInfo = new DirectoryInfo(Environment.CurrentDirectory + "\\Resources\\" + "\\Images\\");
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                if (fileInfo.Extension.ToLower() == ".jpg")
                {
                    if (!isInCache)
                    {
                        _images.Add(new BitmapImage(new Uri(fileInfo.FullName)));
                    }
                }
            }
        }

        private void PictureTimer_Tick(object sender, EventArgs e)
        {
            _imageNumber = _rand.Next(_images.Count);
            SlideshowBlock.Source = _images[_imageNumber];
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Papers_Click(object sender, RoutedEventArgs e)
        {
            _ = (NavigationService?.Navigate(new Contents()));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _pictureTimer.Stop();
            isInCache = true;
        }
    }

}
