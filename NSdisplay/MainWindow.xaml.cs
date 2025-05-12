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
        public MainWindow(int id)
        {
            InitializeComponent();

            con.Open();

            int stationID = id;
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
            await Task.Delay(TimeSpan.FromSeconds(1));
            double width = reviewContainer.ActualWidth;
            ThicknessAnimation reviewAnimation = new ThicknessAnimation(new Thickness(0, 0, 0, 20), new Thickness(-width/2,0,0,20), TimeSpan.FromSeconds(1));
            reviewAnimation.EasingFunction = new SineEase();
            while (true)
            {
                reviewContainer.BeginAnimation(MarginProperty, reviewAnimation);
                await Task.Delay(TimeSpan.FromSeconds(5));
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
}