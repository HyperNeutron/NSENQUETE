
using System.Windows;
using Administration.Models;
using Administration.Services.Database;
using MySql.Data.MySqlClient;

namespace Administration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int StationID { get; set; }
        public string StationName { get; set; }
        public string UserName { get; set; }
        private int currentReviewId { get; set; }
        public MainWindow(int stationID, string stationName, User user)
        {

            InitializeComponent();
            this.DataContext = this;
            this.StationName = stationName;
            this.UserName = user.Username;

            Loaded += async (s, e) =>
            {
                await LoadReviewData();
            };
        }
        private async void Accept_Feedback(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
            "Weet je zeker dat je deze review wilt goedkeuren?",
            "Bevestig",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AcceptButton.IsEnabled = false;
                RejectButton.IsEnabled = false;

                await ProcessFeedback(true);

                MessageBox.Show(
                    "Review goedgekeurd!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                AcceptButton.IsEnabled = true;
                RejectButton.IsEnabled = true;
            }
        }
        private async void Deny_Feedback(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Weet je zeker dat je deze review wilt afkeuren?",
                "Bevestig",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AcceptButton.IsEnabled = false;
                RejectButton.IsEnabled = false;

                await ProcessFeedback(false);

                MessageBox.Show(
                    "Review afgekeurd!",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                AcceptButton.IsEnabled = true;
                RejectButton.IsEnabled = true;
            }
        }

        private async Task LoadReviewData()
        {
            while (true)
            {
                (int id, string name, string short_story, string feedback)? response = null;

                try
                {
                    await Task.Run(() =>
                    {
                        using (MySqlConnection connection = DbConnection.GetConnection())
                        {
                            connection.Open();

                            string query = "SELECT * FROM user_feedback WHERE station = @station LIMIT 1";
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@station", StationName);
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        response = (
                                            reader.GetInt32(reader.GetOrdinal("id")),
                                            reader.GetString(reader.GetOrdinal("name")),
                                            reader.GetString(reader.GetOrdinal("small_story")),
                                            reader.GetString(reader.GetOrdinal("feedback"))
                                        );
                                    }
                                }
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (response == null)
                {
                    error.Opacity = 1;
                    feedbackContent.Opacity = 0;

                    await Task.Delay(10000);
                }
                else
                {
                    error.Opacity = 0;
                    feedbackContent.Opacity = 1;

                    var (id, reviewName, reviewShortStory, reviewFeedback) = response.Value;

                    currentReviewId = id;

                    name.Text = reviewName;
                    shortStory.Text = reviewShortStory;
                    feedback.Text = reviewFeedback;

                    break;
                }
            }
        }

        private async Task ProcessFeedback(bool isApproved)
        {
            string nameValue = name.Text;
            string shortStoryValue = shortStory.Text;
            string feedbackValue = feedback.Text;

            try
            {
                await Task.Run(() =>
                {

                    using (MySqlConnection connection = DbConnection.GetConnection())
                    {
                        connection.Open();

                        string query = "INSERT INTO processed_feedback (name, small_story, feedback, station, is_approved, approved_by, approved_on) VALUES (@name, @small_story, @feedback, @station, @is_approved, @approved_by, @approved_on)";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@name", nameValue);
                            command.Parameters.AddWithValue("@small_story", shortStoryValue);
                            command.Parameters.AddWithValue("@feedback", feedbackValue);
                            command.Parameters.AddWithValue("@station", StationID);
                            command.Parameters.AddWithValue("@approved_by", UserName);
                            command.Parameters.AddWithValue("@is_approved", isApproved);
                            command.Parameters.AddWithValue("@approved_on", DateTime.Now);

                            command.ExecuteNonQuery();
                        }
                    }

                    using (MySqlConnection connection = DbConnection.GetConnection())
                    {
                        connection.Open();

                        string query = "DELETE FROM user_feedback WHERE id = @id";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", currentReviewId);
                            command.ExecuteNonQuery();
                        }
                    }

                });
            }
            catch
            {
                MessageBox.Show("Er was een onverwachte fout bij het versturen van de data. Probeer opnieuw.",
                                 "Error",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
            }

            var newWindow = new MainWindow(this.StationID, this.StationName, new User { Username = this.UserName });
            newWindow.Show();
            this.Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Login();
            loginWindow.Show();

            this.Close();
        }
    }
}