using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSK.Končar.WeatherApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class alter_search_results_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeatherData",
                table: "Results",
                newName: "IconId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Results",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TemperatureCelsius",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "TemperatureCelsius",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "IconId",
                table: "Results",
                newName: "WeatherData");
        }
    }
}
