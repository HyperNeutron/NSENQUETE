using System.Windows;
using MySql.Data.MySqlClient;
using NSdisplay.Services.Database;

namespace NSdisplay;

public partial class feedback : Window
{
    private static MySqlConnection conn = DbConnection.GetConnection();
    private int stationId;
    
    public feedback(int stationId)
    {
        conn.Open();
        InitializeComponent();
        this.stationId = stationId;
        ChangeStationname();
    }

    private void ChangeStationname()
    {
        const string query = "SELECT id, name FROM netherlands_train_stations WHERE id = @stationID";
        var cmd = new MySqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@stationID", stationId);
        var reader = cmd.ExecuteReader();
        if (!reader.Read()) return;
        var stationName = reader.GetString(1);
        StationDisplay.Text = stationName;
    }
    
    private void OnNameEntered(object sender, RoutedEventArgs e)
    {
        var name = NameInput.Text;

        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Anoniem";
        }
        
        NameDisplay.Text = name;
        NameDisplay.Visibility = Visibility.Visible;
        AskForName.Visibility = Visibility.Collapsed;
    }
}

public class FeedbackModel
{
    public string Name { get; set; }
    public string shortstory { get; set; }
    public string feedback { get; set; }
}