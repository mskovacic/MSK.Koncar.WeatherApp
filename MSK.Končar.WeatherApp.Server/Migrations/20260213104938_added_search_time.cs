using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSK.Končar.WeatherApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class added_search_time : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SearchDateTime",
                table: "Searches",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WeatherConditionExternalId",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchDateTime",
                table: "Searches");

            migrationBuilder.DropColumn(
                name: "WeatherConditionExternalId",
                table: "Results");
        }
    }
}
