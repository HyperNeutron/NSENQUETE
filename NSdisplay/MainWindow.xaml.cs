using System.Data.Common;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class DBConnection
    {
        private DBConnection()
        {
        }

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MySqlConnection Connection { get; set; }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Connection.Close();
        }
    }
    public partial class MainWindow : Window
    {
        public static DBConnection DBcon = DBConnection.Instance();
        public MainWindow(int id)
        {
            int stationID = id;
            string stationName;
            DBcon.Server = "localhost";
            DBcon.DatabaseName = "ns";
            DBcon.UserName = "root";
            InitializeComponent();
            if (DBcon.IsConnect())
            {
                string query = "SELECT id, name FROM netherlands_train_stations WHERE id = @stationID";
                var cmd = new MySqlCommand(query, DBcon.Connection);
                cmd.Parameters.AddWithValue("@stationID", stationID);
                var reader = cmd.ExecuteReader();
                station.Text = "bbb";
                if (reader.Read())
                {
                    stationName = reader.GetString(1);
                    station.Text = stationName;
                }
                reader.Close();
            }
        }
    }
}