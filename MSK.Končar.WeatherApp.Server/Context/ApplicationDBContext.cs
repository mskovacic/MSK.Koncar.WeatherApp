using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace MSK.Končar.WeatherApp.Server.Context
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var lines = File.ReadAllLines("data/worldcities.csv");
            var cities = lines
            .Skip(1)
            .Select((line, id) =>
            {
                Span<Range> ranges = stackalloc Range[10];
                ReadOnlySpan<char> span = line.AsSpan();
                span.Split(ranges, ',', StringSplitOptions.TrimEntries);

                return new City
                {
                    Id = -id - 1,
                    Name = span[ranges[0]].Trim('"').ToString(),
                    Latitude = double.Parse(span[ranges[2]].Trim('"'), CultureInfo.InvariantCulture),
                    Longitude = double.Parse(span[ranges[3]].Trim('"'), CultureInfo.InvariantCulture),
                    Country = span[ranges[4]].Trim('"').ToString(),
                    State = span[ranges[7]].Trim('"').ToString(),
                    OriginalName = span[ranges[1]].Trim('"').ToString()
                };
            })
            .ToArray();

            builder.Entity<City>().HasData(cities);
        }

        public DbSet<CityWeatherSearch> Searches => Set<CityWeatherSearch>();

        public DbSet<City> Cities => Set<City>();

        public DbSet<CityWeatherSearchResult> Results => Set<CityWeatherSearchResult>();

    }
}
