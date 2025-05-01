using System.Data;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace NS.Helpers;

public class DataBaseHelper
{
    private static readonly string _connectionString = "server=localhost;uid=root;pwd=;database=nsenquete";
    
    // Stuur ingezende feedback naar een database om beoordeeld te worden
    public static void SendUserFeedback(string name, string smallStory, string feedback)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO user_feedback (name, small_story, feedback, station, feedback_timestamp) VALUES (@name, @small_story, @feedback, @station, @feedback_timestamp)";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@small_story", smallStory);
                command.Parameters.AddWithValue("@feedback", feedback);
                command.Parameters.AddWithValue("@station", Program.StationName);
                command.Parameters.AddWithValue("@feedback_timestamp", DateTime.Now);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }
    }

    // Haal feedback op om te beoordelen
    public static (int, string?, string?, string?) GetReviewRow()
    {
        (int id, string? name, string? smallStory, string? feedback) response = (0, null, null, null);

        using MySqlConnection connection = new MySqlConnection(_connectionString);  
        
        connection.Open();

        string query = "SELECT * FROM user_feedback WHERE station = @station LIMIT 1";
        using MySqlCommand command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@station", Program.StationName);
        MySqlDataReader data = command.ExecuteReader();
        if (data.Read())
        {
            response = (data.GetInt32(data.GetOrdinal("id")), data.GetString(data.GetOrdinal("name")), data.GetString(data.GetOrdinal("small_story")), data.GetString(data.GetOrdinal("feedback")));
        }
        return response;
    }

    // Stuur beoordeelde feedback naar een nieuwe database
    public static void SendProcessedFeedback(int reviewID, string name, string smallStory, string feedback, bool isApproved)
    {
        using (MySqlConnection connection = new(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO processed_feedback (name, small_story, feedback, station, is_approved, approved_on) VALUES (@name, @small_story, @feedback, @station, @is_approved, @approved_on)";
            using MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@small_story", smallStory);
            command.Parameters.AddWithValue("@feedback", feedback);
            command.Parameters.AddWithValue("@station", Program.StationName);
            command.Parameters.AddWithValue("@is_approved", isApproved);
            command.Parameters.AddWithValue("@approved_on", DateTime.Now);

            command.ExecuteNonQuery();
        }
        RemoveFromUserFeedback(reviewID);
    }

    // Verwijder beoordeelde feedback uit de oude database
    public static void RemoveFromUserFeedback(int reviewID)
    {
        using MySqlConnection connection = new(_connectionString);
        connection.Open();

        string query = "DELETE FROM user_feedback WHERE id = @id";
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@id", reviewID);
        command.ExecuteNonQuery();
    }
}