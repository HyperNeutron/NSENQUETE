using MySql.Data.MySqlClient;
using NSdisplay.services.Database;

namespace NSdisplay.Services.Database;

public static class DbConnection
{
    private static readonly string connectionString =
        $"Server={Config.Host};Database={Config.Name};Uid={Config.User};Pwd={Config.Pass}";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}