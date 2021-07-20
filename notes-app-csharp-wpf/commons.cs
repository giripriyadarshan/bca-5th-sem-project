using System.Data.SqlClient;

namespace notes_app_csharp_wpf
{
    class commons
    {
        public static string connstring = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|vapdb.mdf;Integrated Security=True;Connect Timeout=30";
        public static SqlConnection connection = new SqlConnection(connstring);
        public static SqlCommand command = new SqlCommand();
        public static SqlDataAdapter da = new SqlDataAdapter(command);

        public static void Set_Command(string query)
        {
            command.Connection = connection;
            command.CommandText = query;
        }
    }
}
