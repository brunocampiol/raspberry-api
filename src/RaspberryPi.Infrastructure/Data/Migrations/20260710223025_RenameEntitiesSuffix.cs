using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaspberryPi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntitiesSuffix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "EmailsOutbox",
                schema: null,
                newName: "EmailOutboxEntities",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "Facts",
                schema: null,
                newName: "FactEntities",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "FeedbackMessages",
                schema: null,
                newName: "FeedbackMessageEntities",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "GeoLocations",
                schema: null,
                newName: "GeoLocationEntities",
                newSchema: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "EmailOutboxEntities",
                schema: null,
                newName: "EmailsOutbox",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "FactEntities",
                schema: null,
                newName: "Facts",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "FeedbackMessageEntities",
                schema: null,
                newName: "FeedbackMessages",
                newSchema: null);

            migrationBuilder.RenameTable(
                name: "GeoLocationEntities",
                schema: null,
                newName: "GeoLocations",
                newSchema: null);
        }
    }
}
