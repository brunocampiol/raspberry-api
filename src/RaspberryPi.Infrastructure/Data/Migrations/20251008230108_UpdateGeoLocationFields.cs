using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspberryPi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeoLocationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "GeoLocations");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "GeoLocations",
                newName: "LocationName");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "GeoLocations",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationName",
                table: "GeoLocations",
                newName: "City");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "GeoLocations",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "GeoLocations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
