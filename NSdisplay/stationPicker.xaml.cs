using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using NSdisplay.Services.Database;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class stationPicker : Window
    {
        class station
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        public static MySqlConnection con = DbConnection.GetConnection();

        public stationPicker()
        {
            con.Open();
            InitializeComponent();
            List<station> stations = new List<station>();
            string query = "SELECT id, name FROM netherlands_train_stations";
            var cmd = new MySqlCommand(query, con);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                stations.Add(new station { id = reader.GetInt32(0), name = reader.GetString(1) });
            }
            reader.Close();
            stations = stations.OrderBy(s => s.name).ToList();
            selectedStation.ItemsSource = stations;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int stationID = (int)selectedStation.SelectedValue;
                MainWindow mainwindow = new MainWindow(stationID);
                mainwindow.Show();
                con.Close();
                Close();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Station niet gevonden");
            }
        }

        private void selectedStation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            selectedStation.IsDropDownOpen = true;
        }
    }
}
