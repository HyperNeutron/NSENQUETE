
using System.Windows;

using System.Windows.Input;

using Administration.Models;
using Administration.Services.Database;
using MySql.Data.MySqlClient;

namespace Administration
{
    /// <summary>
    /// Interaction logic for StationPicker.xaml
    /// </summary>
    public partial class StationPicker : Window
    {
        public static MySqlConnection con = DbConnection.GetConnection();
        int stationID;
        string stationName;
        User currentUser;

        public StationPicker(User currentUser)
        {
            this.currentUser = currentUser;
            con.Open();
            InitializeComponent();
            var stations = new List<Station>();
            var query = "SELECT id, name FROM stations";
            var cmd = new MySqlCommand(query, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                stations.Add(new Station { id = reader.GetInt32(0), name = reader.GetString(1) });
            }
            reader.Close();
            stations = stations.OrderBy(s => s.name).ToList();
            selectedStation.ItemsSource = stations;
            selectedStation.KeyDown += SelectedStation_KeyDown;
        }

        private void SelectedStation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (selectedStation.SelectedValue != null && isValidStation((int)selectedStation.SelectedValue))
                {
                    Station SelectedStation = (Station)selectedStation.SelectedItem;
                    stationID = SelectedStation.id;
                    stationName = SelectedStation.name;
                    switchWindow();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStation.SelectedValue != null && isValidStation((int)selectedStation.SelectedValue))
            {
                Station SelectedStation = (Station)selectedStation.SelectedItem;
                stationID = SelectedStation.id;
                stationName = SelectedStation.name;
                switchWindow();
            }
            else
            {
                error.Text = "Station niet gevonden";
            }
        }

        private bool isValidStation(int id)
        {
            var query = "SELECT * FROM stations WHERE id = @id";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        private void switchWindow()
        {
            var mainwindow = new MainWindow(stationID, stationName, currentUser);
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
