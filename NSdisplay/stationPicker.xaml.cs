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
using static System.Collections.Specialized.BitVector32;

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
        public static DBConnection DBcon = DBConnection.Instance();
        public stationPicker()
        {
            InitializeComponent();
            DBcon.Server = "localhost";
            DBcon.DatabaseName = "ns";
            DBcon.UserName = "root";
            List<station> stations = new List<station>();
            if (DBcon.IsConnect())
            {
                string query = "SELECT id, name FROM netherlands_train_stations";
                var cmd = new MySqlCommand(query, DBcon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    stations.Add(new station { id = reader.GetInt32(0), name = reader.GetString(1) });
                    text.Text = reader.GetString(1);
                }
                reader.Close();
                stations = stations.OrderBy(s => s.name).ToList();
                selectedStation.ItemsSource = stations;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int stationID = (int)selectedStation.SelectedValue;
            MainWindow mainwindow = new MainWindow(stationID);
            mainwindow.Show();
            Close();
        }

        private void selectedStation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            selectedStation.IsDropDownOpen = true;
        }
    }
}
