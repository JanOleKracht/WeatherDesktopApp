using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Threading;
using Newtonsoft.Json;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // API Key and URL from where weather data is loaded
        private string apiKey = Environment.GetEnvironmentVariable("WEATHER_API_KEY", EnvironmentVariableTarget.User);

        private string currentWeatherURL = "https://api.openweathermap.org/data/2.5/weather";
        private string hourlyForecastUR = "https://api.openweathermap.org/data/2.5/forecast";

        // Creating an HTTP client
        private HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            string city = "Braunschweig";
            InitializeWeatherData(city);
            DisplayFirstFiveTemperaturesConditions(city);
        }

        // Method to load the current weather data from the website into the program. Async is asynchronous.
        // Allows smooth use of the application even when data is being fetched. Code runs alongside other code
        public async Task<WeatherMapResponse> GetWeatherData(string city)
        {
            try
            {
                var finalUri = currentWeatherURL + "?q=" + city + "&appid=" + apiKey + "&units=metric" + "&lang=en";
                HttpResponseMessage httpResponse = await httpClient.GetAsync(finalUri);
                string response = await httpResponse.Content.ReadAsStringAsync();
                // Json is a JavaScript file. To use it in C#, it must be converted.
                WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
                return weatherMapResponse;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network Error: {ex.Message}");
                return null;
            }
        }

        // Method to load the weather forecast data from the website into the program
        public async Task<WeatherMapResponse> GetWeatherForcast(string city)
        {
            var finalUri = hourlyForecastUR + "?q=" + city + "&appid=" + apiKey + "&units=metric" + "&lang=en";
            HttpResponseMessage httpResponse = await httpClient.GetAsync(finalUri);
            string response = await httpResponse.Content.ReadAsStringAsync();
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
            return weatherMapResponse;
        }

        // Method to initialize weather data and conditions
        private async Task InitializeWeatherData(string city)
        {
            try
            {
                // Retrieve data from GetWeatherData to process it
                WeatherMapResponse result = await GetWeatherData(city);

                // Check if the city exists. If not, return an error message
                if (result == null || result.weather == null)
                {
                    //MessageBox.Show("Stadt nicht vorhanden");
                    textSearchBoxCity.Text = "Please enter an existing City";
                    labelCityLoad.Content = " ";
                    return;
                }
                else
                {
                    // Display data: temperature, feels-like temperature, weather condition, time, and background image
                    labelTemperature.Content = $"{result.main.temp:F1}°C";
                    labelTemperatureFeel.Content = $"Feels Like Temperature {result.main.feels_like.ToString("F1")}°C";
                    labelHumidity.Content = $"Humidityt: {result.main.humidity}%";
                    labelTemperatureMax.Content = $"Maximum Temperature {result.main.temp_max:F1}°C";
                    labelTemperatureMin.Content = $"Minimum Temperature {result.main.temp_min:F1}°C";
                    labelDescription.Content = result.weather[0].description;

                    GetTime(city);
                    BackgroundImage(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{textSearchBoxCity.Text} does not exist. Please enter another city.\nError: {ex.Message}",
                    "City Not Found",
                    MessageBoxButton.OK);
            }
        }

        // Method to set background image based on weather conditions
        private void BackgroundImage(WeatherMapResponse result)

        {
            // Display image based on condition: cloud, rain, snow, else (sun)
            string finalImage = "sun.png";
            string currentWeather = result.weather[0].main.ToLower();

            if (currentWeather.Contains("cloud"))
            {
                finalImage = "cloud.png";
            }
            else if (currentWeather.Contains("rain") || currentWeather.Contains("drizzle"))
            {
                finalImage = "rain.png";
            }
            else if (currentWeather.Contains("snow"))
            {
            }
            else
            {
                finalImage = "sun.png";
            }

            // Set background image in the app
            backroundImage.ImageSource = new BitmapImage(new Uri("Images/" + finalImage, UriKind.Relative));
        }

        // Method for time display
        public async void GetTime(string city)
        {
            WeatherMapResponse result = await GetWeatherData(city);
            // result.timezone is never null. Left as is, because != 0 does not work since the timezone in England is 0, causing no value to be displayed
            if (result.timezone != null)
            {
                DateTime utcNow = DateTime.UtcNow;
                float timezoneOffset = result.timezone; // Assuming this is in seconds
                DateTime localTime = utcNow.AddSeconds(timezoneOffset);
                labelTime.Content = $"{localTime:HH:mm}";
            }
            else
            {
                labelTime.Content = "Zeit zurzeit nicht verfügbar";
            }
        }

        // Method to predict the next 5 temperatures and conditions

        public async void DisplayFirstFiveTemperaturesConditions(string city)
        {
            WeatherMapResponse weatherMapResponse = await GetWeatherForcast(city);

            // HashSet ensures each element is included only once

            HashSet<string> seenDates = new HashSet<string>();

            int count = 0;
            // Collect temperatures of the first five forecasts in a loop
            string forecastData = "";

            if (weatherMapResponse.list == null)
            {
                // No box displayed, otherwise it would appear twice
            }
            else
            {
                for (int i = 0; i < weatherMapResponse.list.Count && count < 5; i++)
                {
                    var forecast = weatherMapResponse.list[i];
                    var temperature = forecast.main.temp;
                    var weatherCondition = forecast.weather[0].description.ToLower();

                    // Create weekday and date
                    var dateText = forecast.dt_txt; // Beispiel: "2023-12-01 12:00:00" dt_txt ist die Bezeichnung in JSON
                    var dateTime = DateTime.Parse(dateText); // In ein DateTime-Objekt konvertieren
                    var dayOfWeek = dateTime.ToString("ddd"); // Vollständiger Wochentag

                    // Since multiple temperature values per day exist in the list, HashSet checks if a date already exists.
                    // HashSet ensures each element is included only once. !seenDates = If a date is not yet present, it is added to the list
                    string dateKey = dateTime.ToString("yyyy-MM-dd");
                    if (!seenDates.Contains(dateKey))
                    {
                        seenDates.Add(dateKey);

                        // Add forecast data with weekday and temperature
                        forecastData += $"{dayOfWeek} {dateTime:dd.MM}  {temperature:F1}°C  {weatherCondition} \r\n";

                        count++;
                    }
                }
                labelForecastData.Content = forecastData;
            }
        }

        // Programierung von Buttons und TextBoxen
        private void clickButtonForCity_Click(object sender, RoutedEventArgs e)
        {
            string city = textSearchBoxCity.Text;
            InitializeWeatherData(city);
            DisplayFirstFiveTemperaturesConditions(city);
            labelCityLoad.Content = char.ToUpper(city[0]) + city.Substring(1);
        }

        private void Press_Enter(object sender, KeyEventArgs e)
        {
            // Check if the Enter key was pressed
            if (e.Key == Key.Enter)
            {
                clickButtonForCity_Click(sender, e);

                Keyboard.ClearFocus();
            }
        }

        private void TextSearchBoxCity_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Clear();
            }
        }
    }
}