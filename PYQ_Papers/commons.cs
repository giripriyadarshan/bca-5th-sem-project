using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PYQ_Papers.Pages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Controls;

namespace PYQ_Papers
{
    public class commons
    {
        // Database related
        private static readonly string connstring =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|desktopappdb.mdf;Integrated Security=True;Connect Timeout=30";

        public static SqlConnection connection = new SqlConnection(connstring);
        public static SqlCommand command = new SqlCommand();
        public static SqlDataAdapter da = new SqlDataAdapter(command);

        public static void Set_Command(string query)
        {
            command.Connection = connection;
            command.CommandText = query;
        }

        public static string Set_File_Storage_String(int yearID)
        {
            var _fileStorage = Environment.CurrentDirectory + "\\Resources\\" + "\\QPS\\";
            _fileStorage += yearID + "\\";
            return _fileStorage;
        }

        // List all files
        public static List<ListOfFiles> list = new List<ListOfFiles>();

        public static void Add_All_Files(ref ListBox fileList)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // load all files in the beginning
            var allfiles = new DataTable();
            Set_Command("SELECT * FROM files");
            _ = da.Fill(allfiles);


            list.Clear();
            fileList.Items.Clear();
            foreach (DataRow dtrow in allfiles.Rows)
            {
                list.Add(new ListOfFiles()
                {
                    PathOfFile = dtrow["file_name"].ToString(),
                    YearID = dtrow["yearID"].ToString(),
                });
            }

            fileList.ItemsSource = list;

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // Search related function using iText7 library
        public static string ReadFile(string pdfPath)
        {
            var pageText = new StringBuilder();
            using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(pdfPath)))
            {
                var pageNumbers = pdfDocument.GetNumberOfPages();
                for (int i = 1; i <= pageNumbers; i++)
                {
                    LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                    parser.ProcessPageContent(pdfDocument.GetFirstPage());
                    pageText.Append(strategy.GetResultantText());
                }
            }

            return pageText.ToString();
        }
    }

    public static class Session
    {
        public static int adminId = -1;
        public static string adminName = null;
        public static bool isLoggedIn = false;
    }
}