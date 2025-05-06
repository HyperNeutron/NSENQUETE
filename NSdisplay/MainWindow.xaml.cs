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
using System.ComponentModel;

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
            string query = "SELECT name, hasLift, wheelChairAccessible, hasToilet, haskiosk FROM netherlands_train_stations WHERE id = @stationID";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@stationID", stationID);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                stationName = reader.GetString(0);
                stationLabel.Text = stationName;
                var disabledColor = new Color();
                disabledColor.R = 0;
                disabledColor.G = 0;
                disabledColor.B = 77;
                disabledColor.A = 255;
                var disabledBrush = new SolidColorBrush(disabledColor);
                if (!reader.GetBoolean(1))
                {
                    elevatorIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(2))
                {
                    wheelchairIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(3))
                {
                    toiletIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(4))
                {
                    kioskIcon.Foreground = disabledBrush;
                }
            }
            reader.Close();
            updateClock();
        }

        async void updateClock()
        {
            while (true)
            {
                clock.Text = DateTime.Now.ToString("HH:mm");
                await Task.Delay(TimeSpan.FromMilliseconds(250));
            }

        }
    }
}