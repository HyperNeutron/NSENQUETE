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
        int stationID;
        public stationPicker()
        {
            try
            {
                con.Open();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Database niet verbonden", "Error");
                App.Current.Shutdown();
                return;
            }
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
                    stationID = (int)selectedStation.SelectedValue;
                    switchWindow();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedStation.SelectedValue != null && isValidStation((int)selectedStation.SelectedValue))
            {
                stationID = (int)selectedStation.SelectedValue;
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
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return true;
                }
                return false;
            }
        }

        private void switchWindow()
        {
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
