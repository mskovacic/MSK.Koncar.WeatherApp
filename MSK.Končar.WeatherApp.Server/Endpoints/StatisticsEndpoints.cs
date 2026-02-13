using MSK.Končar.WeatherApp.Server.Models.Responses;
using System.Security.Claims;

namespace MSK.Končar.WeatherApp.Server.Endpoints
{
    public static class StatisticsEndpoints
    {
        public static void MapStatisticsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("statistics/top-cities", GetTopCitiesAsync);

            app.MapGet("statistics/latest-searches", GetLatestSearchesAsync);

            app.MapGet("statistics/weather-conditions-distribution", GetWeatherConditionsDistributionAsync);
        }

        public static async Task<IResult> GetTopCitiesAsync(
            ApplicationDBContext dBContext,
            HttpContext context)
        {
            var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var topCities = dBContext.Searches
                .Where(s => s.UserId == userId)
                .Select(s => s.City.Name)
                .GroupBy(c => c).Select(g => new
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(3);

            return Results.Ok(topCities);
        }

        public static async Task<IResult> GetLatestSearchesAsync(
            ApplicationDBContext dBContext,
            HttpContext context)
        {
            var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var latestSearches = dBContext.Searches
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SearchDateTime)
                .Take(3)
                .Select(s => new
                {
                    City = s.City.Name,
                    SearchDate = s.SearchDateTime,
                    Forecast = s.SearchResults.Select(r => new WeatherResponse
                    {
                        DateTime = r.DateTime,
                        Description = r.Description,
                        IconURL = $"https://openweathermap.org/payload/api/media/file/{r.IconId}.png",
                        TemperatureCelsius = r.TemperatureCelsius
                    })
                });

            return Results.Ok(latestSearches);
        }

        public static async Task<IResult> GetWeatherConditionsDistributionAsync(
            ApplicationDBContext dBContext,
            HttpContext context)
        {
            var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var distribution = dBContext.Results
                .Where(s => s.CityWeatherSearch.UserId == userId)
                .GroupBy(r => r.WeatherConditionExternalId)
                .Select(g => new
                {
                    Condition = g.First().Description,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count);

            return Results.Ok(distribution);
        }
    }
}
