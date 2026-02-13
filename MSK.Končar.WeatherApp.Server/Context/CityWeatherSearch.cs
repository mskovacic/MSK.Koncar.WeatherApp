using Microsoft.AspNetCore.Identity;

namespace MSK.Končar.WeatherApp.Server.Context
{
    public class CityWeatherSearch
    {
        public required int Id { get; set; }

        public required string UserId { get; set; }

        public IdentityUser User { get; set; } = null!;

        public required int CityId { get; set; }

        public City City { get; set; } = null!;

        public DateTime SearchDateTime { get; set; } = DateTime.UtcNow;

        public ICollection<CityWeatherSearchResult> SearchResults { get; set; } = [];
    }
}
