using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;
using NSdisplay.Services.Database;

namespace Feedback
{

    public partial class MainWindow : Window
    {
        private static MySqlConnection conn = DbConnection.GetConnection();
        private static int SmallStoryCount = 100;
        private static int FeedbackCount = 500;
        private FeedbackModel feedbackModel = new FeedbackModel();
        private int stationId;

        public MainWindow(int stationId)
        {
            conn.Open();
            InitializeComponent();
            this.stationId = stationId;
            ChangeStationName();

            FeedbackInput.TextChanged += FeedbackInput_TextChanged;
            SmallStoryInput.TextChanged += ShortStoryInput_TextChanged;

            NameDisplay.Visibility = Visibility.Collapsed;
            AskForSmallStory.Visibility = Visibility.Collapsed;
            AskForFeedback.Visibility = Visibility.Collapsed;
        }
        private void SaveFeedback()
        {
            const string query = "INSERT INTO user_feedback (sender, shortMessage, longMessage, station_id) VALUES (@name, @message, @feedback, @station_id)";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", feedbackModel.Name ?? "Anonymous");
            cmd.Parameters.AddWithValue("@message", feedbackModel.shortstory ?? string.Empty);
            cmd.Parameters.AddWithValue("@feedback", feedbackModel.feedback ?? string.Empty);
            cmd.Parameters.AddWithValue("@station_id", stationId);

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error saving feedback: {ex.Message}");
            }
        }

        private void ChangeStationName()
        {
            const string query = "SELECT id, name FROM stations WHERE id = @stationID";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@stationID", stationId);

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read()) StationDisplay.Text = reader.GetString("name");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error loading station: {ex.Message}");
                StationDisplay.Text = "Unknown Station";
            }
        }

        private void OnNameEntered(object sender, RoutedEventArgs e)
        {
            var name = NameInput.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Anoniem";
            }

            NameDisplay.Text = name;
            feedbackModel.Name = name;
            NameDisplay.Visibility = Visibility.Visible;
            AskForName.Visibility = Visibility.Collapsed;
            AskForSmallStory.Visibility = Visibility.Visible;
        }

        private void ShortStoryInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var charCount = SmallStoryInput.Text.Length;
            SSCharCounter.Text = $"{charCount}/{SmallStoryCount}";

            SSCharCounter.Foreground = charCount >= SmallStoryCount ? Brushes.Red : Brushes.Gray;
        }

        private void OnShortStoryEntered(object sender, RoutedEventArgs e)
        {
            var shortStory = SmallStoryInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(shortStory))
            {
                MessageBox.Show("Vul minimaal 1 karakter in voor het bericht", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (shortStory.Length > SmallStoryCount)
            {
                MessageBox.Show($"Het bericht mag maximaal {SmallStoryCount} karakters bevatten", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            feedbackModel.shortstory = shortStory;
            AskForSmallStory.Visibility = Visibility.Collapsed;
            AskForFeedback.Visibility = Visibility.Visible;

        }

        private void OnShortStoryBackBtn(object sender, RoutedEventArgs e)
        {
            AskForSmallStory.Visibility = Visibility.Collapsed;
            AskForName.Visibility = Visibility.Visible;
            NameDisplay.Visibility = Visibility.Collapsed;
        }

        private void FeedbackInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var charCount = FeedbackInput.Text.Length;
            FBCharCounter.Text = $"{charCount}/{FeedbackCount}";

            FBCharCounter.Foreground = charCount >= FeedbackCount ? Brushes.Red : Brushes.Gray;
        }

        private void OnFeedbackEntered(object sender, RoutedEventArgs e)
        {
            var feedback = FeedbackInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(feedback))
            {
                MessageBox.Show("Vul minimaal 1 karakter in voor feedback", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (feedback.Length > FeedbackCount)
            {
                MessageBox.Show("$Feedback mag maximaal {FeedbackCount} karakters bevatten", "Fout", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            feedbackModel.feedback = feedback;
            SaveFeedback();
            MessageBox.Show(
                "Dankjewel voor je waardevolle feedback! \n\nWe gaan er direct mee aan de slag.",
                "❤️ Feedback ontvangen",
                MessageBoxButton.OK);
            conn.Close();
            var feedbackWindow = new MainWindow(stationId);
            feedbackWindow.Show();
            Close();
        }

        private void OnFeedbackBackBtn(object sender, RoutedEventArgs e)
        {
            AskForFeedback.Visibility = Visibility.Collapsed;
            AskForSmallStory.Visibility = Visibility.Visible;
        }
    }

    public class FeedbackModel
    {
        public string Name { get; set; }
        public string shortstory { get; set; }
        public string feedback { get; set; }
    }
}