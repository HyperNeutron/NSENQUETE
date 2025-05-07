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
}