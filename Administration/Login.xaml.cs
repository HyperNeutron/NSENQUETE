
using System.Text;
using System.Windows;
using System;
using System.Security.Cryptography;
using System.Windows.Controls;
using Administration.Models;
using Administration.Services.Database;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace Administration
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private int userID;
        public Login()
        {
            InitializeComponent();
            ShowLoginView();
        }

        private void ShowLoginView()
        {
            headerText.Text = "Inloggen";
            loginPanel.Visibility = Visibility.Visible;
            signupPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowSignupView()
        {
            headerText.Text = "Registreren";
            signupPanel.Visibility = Visibility.Visible;
            loginPanel.Visibility = Visibility.Collapsed;
        }

        private void SwitchToSignup_Click(object sender, RoutedEventArgs e)
        {
            ShowSignupView();
        }

        private void SwitchToLogin_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginView();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = loginEmail.Text;
            string password = loginPassword.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vul een email en wachtwoord in!", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (AuthenticateUser(email, password, out string username))
            {
                User currentUser = new User
                {
                    Username = username,
                    Email = email,
                    id = userID,
                };

                var stationPicker = new StationPicker(currentUser);
                stationPicker.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Ongeldig email of wachtwoord.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            string username = signupUsername.Text;
            string email = signupEmail.Text;
            string password = signupPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vul alle velden in!", "Signup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (RegisterUser(username, email, password))
            {
                User currentUser = new User
                {
                    Username = username,
                    Email = email,
                    id = userID
                };

                MessageBox.Show("Account aangemaakt!", "Registration Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                var stationPicker = new StationPicker(currentUser);
                stationPicker.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Registratiefout. Email kan al in gebruik zijn.", "Signup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool AuthenticateUser(string email, string password, out string username)
        {
            username = string.Empty;

            try
            {
                string hashedPassword = HashPassword(password);

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT username,id FROM users WHERE email = @Email AND password = @Password";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        using(var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) {
                                username = reader.GetString(0);
                                userID = reader.GetInt32(1);
                                return true;
                            }
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        private bool RegisterUser(string username, string email, string password)
        {
            try
            {
                if (EmailExists(email))
                {
                    return false;
                }

                string hashedPassword = HashPassword(password);

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "INSERT INTO users (username, email, password) VALUES (@Username, @Email, @Password)";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);

                        int rowsAffected = command.ExecuteNonQuery();
                        userID = unchecked((int)command.LastInsertedId);
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration error: {ex.Message}\n\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        private bool EmailExists(string email)
        {
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM users WHERE email = @Email";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        int count = Convert.ToInt32(command.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking email existence: {ex.Message}\n\nStack Trace: {ex.StackTrace}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}