using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace notes_app_csharp_wpf
{
    class commons
    {
        // Database related
        public static string connstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|vapdb.mdf;Integrated Security=True;Connect Timeout=30";
        public static SqlConnection connection = new SqlConnection(connstring);
        public static SqlCommand command = new SqlCommand();
        public static SqlDataAdapter da = new SqlDataAdapter(command);

        public static void Set_Command(string query)
        {
            command.Connection = connection;
            command.CommandText = query;
        }

        // Other stuffs
        public static bool adminLoginSession = false;
        public static string _fileStorage = Environment.CurrentDirectory + "\\Resources\\" + "\\QPS\\";

        // Images
        public static bool isInCache = false;
        public static readonly List<BitmapImage> _images = new List<BitmapImage>();
        public static int _imageNumber;
        public static readonly DispatcherTimer _pictureTimer = new DispatcherTimer();
        public static readonly Random _rand = new Random();
    }
}
