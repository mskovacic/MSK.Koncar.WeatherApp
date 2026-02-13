using MSK.Končar.WeatherApp.Server.Models.Responses;
using OpenWeatherMap;
using System.Security.Claims;

namespace MSK.Končar.WeatherApp.Server.Endpoints
{
    public static class WeatherEndpoints
    {
        public static void MapWeatherEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("weather/current", GetCurrentWeatherAsync);

            app.MapGet("weather/forecast", Get5DayForecastAsync);
        }

        public static async Task<IResult> GetCurrentWeatherAsync(
            double lattitude,
            double longitude,
            IOpenWeatherMapService client,
            HttpContext context)
        {
            var weather = await client.GetCurrentWeatherAsync(lattitude, longitude);
            if (weather == null)
            {
                return Results.InternalServerError();
            }

            var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var weatherInfo = weather.Weather.FirstOrDefault();
            var response = new CurrentWeatherResponse
            {
                City = weather.CityName,
                Weather = new WeatherResponse
                {
                    DateTime = new DateTimeOffset(weather.Date),
                    TemperatureCelsius = weather.Main.Temperature.DegreesCelsius,
                    Description = weatherInfo?.Description ?? "No description",
                    IconURL = weatherInfo is null ? "sun_question.png" : $"https://openweathermap.org/payload/api/media/file/{weatherInfo.IconId}.png"
                }
            };

            return Results.Ok(response);
        }

        public static async Task<IResult> Get5DayForecastAsync(
            int cityId,
            IOpenWeatherMapService client,
            HttpContext context,
            ApplicationDBContext dBContext)
        {
            var city = await dBContext.Cities.FindAsync(cityId);
            if (city == null)
            {
                return Results.NotFound();
            }

            var forecast = await client.GetWeatherForecast5Async(city.Latitude, city.Longitude);
            if (forecast == null)
            {
                return Results.InternalServerError();
            }

            var userId = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            dBContext.Add(new CityWeatherSearch
            {
                Id = default,
                UserId = userId,
                CityId = cityId,
                SearchResults = forecast.Items.Select(item =>
                {
                    var weatherCondition = item.WeatherConditions.FirstOrDefault();
                    return new CityWeatherSearchResult
                    {
                        Id = default,
                        DateTime = item.DateTime,
                        TemperatureCelsius = item.Main.Temperature.DegreesCelsius,
                        Description = weatherCondition?.Description ?? "No description",
                        IconId = weatherCondition?.IconId ?? "sun_question",
                        CityWeatherSearchId = default,
                        WeatherConditionExternalId = weatherCondition?.Id ?? 0
                    };
                }).ToArray()
            });
            await dBContext.SaveChangesAsync();

            var response = new ForecastResponse
            {
                City = city.Name,
                ForecastCityName = forecast.City.Name,
                Forecast = forecast.Items.Select(item =>
                {
                    var weatherCondition = item.WeatherConditions.FirstOrDefault();
                    return new WeatherResponse
                    {
                        DateTime = new DateTimeOffset(item.DateTime),
                        TemperatureCelsius = item.Main.Temperature.DegreesCelsius,
                        Description = weatherCondition?.Description ?? "No description",
                        IconURL = weatherCondition is null ? "sun_question.png" : $"https://openweathermap.org/payload/api/media/file/{weatherCondition.IconId}.png"
                    };
                }
               ).ToArray()
            };
            return Results.Ok(response);
        }
    }
}
