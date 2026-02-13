namespace MSK.Končar.WeatherApp.Server.Context
{
    public class CityWeatherSearchResult
    {
        public required int Id { get; set; }
        public required int CityWeatherSearchId { get; set; }
        public CityWeatherSearch CityWeatherSearch { get; set; } = null!;
        public required double TemperatureCelsius { get; set; }
        public required DateTime DateTime { get; set; }
        public required string Description { get; set; }
        public required int WeatherConditionExternalId { get; set; }
        public required string IconId { get; set; }
    }
}
