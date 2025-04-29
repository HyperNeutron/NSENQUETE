using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using NSdisplay.Models;
using NSdisplay.Services.Database;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class stationPicker : Window
    {
        public static MySqlConnection con = DbConnection.GetConnection();

        public stationPicker()
        {
            con.Open();
            InitializeComponent();
            var stations = new List<Station>();
            var query = "SELECT id, name FROM netherlands_train_stations";
            var cmd = new MySqlCommand(query, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                stations.Add(new Station { id = reader.GetInt32(0), name = reader.GetString(1) });
            }
            reader.Close();
            stations = stations.OrderBy(s => s.name).ToList();
            selectedStation.ItemsSource = stations;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var stationID = (int)selectedStation.SelectedValue;
            var mainwindow = new MainWindow(stationID);
            mainwindow.Show();
            con.Close();
            Close();
        }

        private void selectedStation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            selectedStation.IsDropDownOpen = true;
        }
    }
}
