using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentBikeApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorcycleNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "motorcycle_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    motorcycle_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    plate = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    received_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_motorcycle_notifications", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motorcycle_notifications");
        }
    }
}
