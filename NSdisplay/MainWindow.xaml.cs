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
using System.Net.Http;
using System.Net.Http.Headers;

namespace NSdisplay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static MySqlConnection con = DbConnection.GetConnection();
        static HttpClient client = new HttpClient();
        int stationID;
        Dictionary<int, string> weatherIcons = new Dictionary<int, string>()
            {
                { 0, "☀️" },
                { 1, "🌤️" },
                { 2, "⛅" },
                { 3, "☁️" },
                { 4, "🌫️" },
                { 5, "🌁" },
                { 6, "🌧️" },
                { 7, "🌨️" },
                { 8, "🌩️" },
                { 9, "🌦️" },
                { 10, "🌧️" },
                { 11, "🌧️" },
                { 12, "🌨️" },
                { 13, "🌨️" },
                { 14, "🌩️" }
            };

        public MainWindow(int id)
        {
            InitializeComponent();

            con.Open();

            stationID = id;
            string stationName;

            client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string query = "SELECT name, hasLift, wheelChairAccessible, hasToilet, hasKiosk FROM stations WHERE id = @stationID";
            var cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@stationID", stationID);

            using (var reader = cmd.ExecuteReader())
            {
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
            }

            updateClock();
            ReviewController();
            updateWeather();
        }

        async void updateClock()
        {
            while (true)
            {
                clock.Text = DateTime.Now.ToString("HH:mm");
                await Task.Delay(TimeSpan.FromMilliseconds(250));
            }
        }

        async void updateWeather()
        {
            while (true)
            {
                try {
                    WeatherReport currentWeather = new();
                    HttpResponseMessage response = await client.GetAsync("forecast?latitude=52.374&longitude=4.8897&current=temperature_2m,weather_code&timezone=Europe%2FBerlin");
                    if (response.IsSuccessStatusCode)
                    {
                        currentWeather = await response.Content.ReadAsAsync<WeatherReport>();
                    }
                    weather.Text = $"{currentWeather.current.temperature_2m} °c {weatherIcons[currentWeather.current.weather_code]}";
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
                catch (HttpRequestException)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        async void ReviewController()
        {
            int feedbackAmount = 5;
            int count = 0;

            //Without this the animation wont work 
            await Task.Delay(TimeSpan.FromMilliseconds(1));

            approvedFeedback[] latestFeedback = new approvedFeedback[feedbackAmount];
            List<approvedFeedback> queue = new();

            double width = reviewContainer.ActualWidth;
            ThicknessAnimation reviewAnimation = new(new Thickness(0, 0, 0, 20), new Thickness(-width / 2, 0, 0, 20), TimeSpan.FromSeconds(2))
            {
                EasingFunction = new SineEase()
            };

            while (true)
            {
                string query = "SELECT sender, shortMessage, date_posted from user_feedback WHERE is_approved AND station_id = @station_id ORDER BY date_posted DESC LIMIT @feedbackAmount";
                var cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@feedbackAmount", feedbackAmount);
                cmd.Parameters.AddWithValue("@station_id", stationID);

                if (queue.Count <= 2)
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        count = 0;
                        while (reader.Read())
                        {
                            latestFeedback[count] = new approvedFeedback(reader.GetString(0), reader.GetString(1), reader.GetDateTime(2));
                            count++;
                        }
                        if(count > queue.Count)
                        {
                            queue.AddRange(latestFeedback.Take(count));
                        }
                    }
                }

                if (count <= 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                reviewContent1.Text = queue.First().small_story;
                reviewer1.Text = queue.First().name;
                date1.Text = queue.First().date_posted.ToString("dd/MM");

                if (count <= 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(10));

                reviewContainer.BeginAnimation(MarginProperty, reviewAnimation);

                reviewContent2.Text = queue[1].small_story;
                reviewer2.Text = queue[1].name;
                date2.Text = queue[1].date_posted.ToString("dd/MM");

                reviewContent1.Text = queue.First().small_story;
                reviewer1.Text = queue.First().name;
                date1.Text = queue.First().date_posted.ToString("dd/MM");

                await Task.Delay(TimeSpan.FromSeconds(3));

                queue.RemoveAt(0);
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

        public approvedFeedback(string name, string small_story, DateTime date_posted)
        {
            this.name = name;
            this.small_story = small_story;
            this.date_posted = date_posted;
        }
    }

    public class WeatherReport
    {
        public Result current;

        public WeatherReport()
        {
            current = new Result();
        }
    }
    public class Result
    {
        public int weather_code;
        public string temperature_2m;
    }
}