using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace NS.Helpers;

public class DataBaseHelper
{
    private static string _connectionString;

    static DataBaseHelper()
    {
        MySql.Data.MySqlClient.MySqlConnection conn;

        try
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection();
            conn.ConnectionString = "server=localhost;uid=root;pwd=admin;database=delta";
            conn.Open();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public static void SendUserFeedBack(string username, string smallStory, string feedBack)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO user_feedback (username, small_story, feedback, station, feedback_timestamp) VALUES (@username, @small_story, @feedback, @station, @feedback_timestamp)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@small_story", smallStory);
                command.Parameters.AddWithValue("@feedback", feedBack);
                command.Parameters.AddWithValue("@station", Program.StationName);
                command.Parameters.AddWithValue("@feedback_timestamp", DateTime.Now);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }
    }
}