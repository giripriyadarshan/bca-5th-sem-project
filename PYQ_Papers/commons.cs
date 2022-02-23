using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PYQ_Papers.Models;
using PYQ_Papers.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PYQ_Papers
{
    public class commons
    {
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
            var context = new Context();
            list.Clear();
            fileList.Items.Clear();
            foreach (var file in context.Files.Include(f => f.Year).ToList())
            {
                list.Add(new ListOfFiles()
                {
                    PathOfFile = file.FileName,
                    YearID = file.Year.Id,
                });
            }

            fileList.ItemsSource = list;

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