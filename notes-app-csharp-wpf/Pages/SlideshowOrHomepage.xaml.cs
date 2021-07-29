using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for SlideshowOrHomepage.xaml
    /// </summary>

    public partial class SlideshowOrHomepage
    {
        private readonly List<BitmapImage> _images = new List<BitmapImage>();
        private int _imageNumber;
        private readonly DispatcherTimer _pictureTimer = new DispatcherTimer();
        private readonly Random _rand = new Random();
        

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
                    _images.Add(new BitmapImage(new Uri(fileInfo.FullName)));
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
    }

}
