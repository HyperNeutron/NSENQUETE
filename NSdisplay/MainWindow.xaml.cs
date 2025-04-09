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
using NSdisplay.Services.Database;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static MySqlConnection con = DbConnection.GetConnection();
        public MainWindow(int id)
        {
            con.Open();
            int stationID = id;
            string stationName;
            InitializeComponent();
            string query = "SELECT id, name FROM netherlands_train_stations WHERE id = @stationID";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@stationID", stationID);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                stationName = reader.GetString(1);
                station.Text = stationName;
            }
            reader.Close();

        }

        
    }
}