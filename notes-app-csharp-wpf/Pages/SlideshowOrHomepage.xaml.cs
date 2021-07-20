using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace notes_app_csharp_wpf.Pages
{
    /// <summary>
    /// Interaction logic for SlideshowOrHomepage.xaml
    /// </summary>

    public partial class SlideshowOrHomepage : Page
    {
        private List<BitmapImage> Images = new List<BitmapImage>();
        private int ImageNumber;
        private DispatcherTimer PictureTimer = new DispatcherTimer();
        private Random rand = new Random();
        

        public SlideshowOrHomepage()
        {
            InitializeComponent();
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PictureTimer.Interval = new TimeSpan(0, 0, 2);
            PictureTimer.Tick += new EventHandler(PictureTimer_Tick);
            PictureTimer.Start();

            DirectoryInfo dir_info = new DirectoryInfo(Environment.CurrentDirectory + "\\Resources\\" + "\\Images\\");
            foreach (FileInfo file_info in dir_info.GetFiles())
            {
                if ((file_info.Extension.ToLower() == ".jpg") ||
                    (file_info.Extension.ToLower() == ".png"))
                {
                    Images.Add(new BitmapImage(new Uri(file_info.FullName)));
                }
            }
        }

        private void PictureTimer_Tick(object sender, EventArgs e)
        {
            ImageNumber = rand.Next(Images.Count);
            SlideshowBlock.Source = Images[ImageNumber];
        }
    }

}
