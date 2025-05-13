using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using MySql.Data.MySqlClient;
using NSdisplay.Services.Database;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static MySqlConnection con = DbConnection.GetConnection();
        int stationID;

        public MainWindow(int id)
        {
            InitializeComponent();

            con.Open();

            stationID = id;
            string stationName;

            string query = "SELECT name, hasLift, wheelChairAccessible, hasToilet, haskiosk FROM netherlands_train_stations WHERE id = @stationID";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@stationID", stationID);
            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                stationName = reader.GetString(0);
                stationLabel.Text = stationName;
                var disabledColor = new Color();
                disabledColor.R = 0;
                disabledColor.G = 0;
                disabledColor.B = 77;
                disabledColor.A = 255;
                var disabledBrush = new SolidColorBrush(disabledColor);
                if (!reader.GetBoolean(1))
                {
                    elevatorIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(2))
                {
                    wheelchairIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(3))
                {
                    toiletIcon.Foreground = disabledBrush;
                }
                if (!reader.GetBoolean(4))
                {
                    kioskIcon.Foreground = disabledBrush;
                }
            }
            reader.Close();

            updateClock();
            nextReview();
        }

        async void updateClock()
        {
            while (true)
            {
                clock.Text = DateTime.Now.ToString("HH:mm");
                await Task.Delay(TimeSpan.FromMilliseconds(250));
            }
        }

        async void nextReview()
        {
            int currentFeedback = 0;
            int nextFeedback = 0;
            int feedbackAmount = 5;

            //Without this the animation wont work 
            await Task.Delay(TimeSpan.FromMilliseconds(1));

            approvedFeedback[] latestFeedback = new approvedFeedback[feedbackAmount];

            //placeholder feedback
            latestFeedback[0] = new approvedFeedback("", "Geen feedback gevonden", new DateTime(0));

            double width = reviewContainer.ActualWidth;
            ThicknessAnimation reviewAnimation = new(new Thickness(0, 0, 0, 20), new Thickness(-width / 2, 0, 0, 20), TimeSpan.FromSeconds(2))
            {
                EasingFunction = new SineEase()
            };

            while (true)
            {
                string query = "SELECT sender, shortMessage, date_posted from messages WHERE is_approved AND station_id = @station_id ORDER BY date_posted DESC LIMIT @feedbackAmount";
                var cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@feedbackAmount", feedbackAmount);
                cmd.Parameters.AddWithValue("@station_id", stationID);
                var reader = cmd.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    latestFeedback[count] = new approvedFeedback(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2));
                    count++;
                }

                reader.Close();

                reviewContent1.Text = latestFeedback[currentFeedback].small_story;
                reviewer1.Text = latestFeedback[currentFeedback].name;
                date1.Text = latestFeedback[currentFeedback].date_posted.ToString("dd/MM");

                if (count <= 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(3));

                reviewContainer.BeginAnimation(MarginProperty, reviewAnimation);

                currentFeedback = nextFeedback;

                if (nextFeedback >= count - 1) nextFeedback = 0;
                else nextFeedback++;

                reviewContent2.Text = latestFeedback[nextFeedback].small_story;
                reviewer2.Text = latestFeedback[nextFeedback].name;
                date2.Text = latestFeedback[nextFeedback].date_posted.ToString("dd/MM");

                reviewContent1.Text = latestFeedback[currentFeedback].small_story;
                reviewer1.Text = latestFeedback[currentFeedback].name;
                date1.Text = latestFeedback[currentFeedback].date_posted.ToString("dd/MM");

                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
    public class DoubleWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value * 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class approvedFeedback
    {
        public string name;
        public string small_story;
        public DateTime date_posted;

        public approvedFeedback(string name, string small_story, DateTime approved_on)
        {
            this.name = name;
            this.small_story = small_story;
            this.date_posted = approved_on;
        }
    }
}