using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Administration.Services.Database
{
    public static class DbConnection
    {
        private static readonly string connectionString =
            $"Server={Config.Host};Database={Config.Name};Uid={Config.User};Pwd={Config.Pass}";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
