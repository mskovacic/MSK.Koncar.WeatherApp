using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MSK.Končar.WeatherApp.Server.Context
{
    [Index(nameof(Name))]
    [Index(nameof(Country))]
    public class City
    {
        public required int Id { get; set; }

        [MaxLength(450)]
        public required string Name { get; set; }

        public required double Latitude { get; set; }
        public required double Longitude { get; set; }

        [MaxLength(450)]
        public required string Country { get; set; }

        public required string State { get; set; }

        public required string OriginalName { get; set; }
    }
}
