namespace MSK.Končar.WeatherApp.Server.Models.Responses
{
    public class CurrentWeatherResponse
    {
        public required string City { get; set; }
        public required WeatherResponse Weather { get; set; }
    }

    public class ForecastResponse
    {
        public required string City { get; set; }
        public required WeatherResponse[] Forecast { get; set; }
    }

    public class WeatherResponse
    {
        public required double TemperatureCelsius { get; set; }
        public required string IconURL { get; set; }
        public required string Description { get; set; }
        public required DateTimeOffset DateTime { get; set; }
    }
}
