using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentBikeApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentals",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    courier_id = table.Column<string>(type: "text", nullable: false),
                    motorcycle_id = table.Column<string>(type: "text", nullable: false),
                    plan_days = table.Column<int>(type: "integer", nullable: false),
                    daily_rate = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    expected_end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    return_date = table.Column<DateOnly>(type: "date", nullable: true),
                    total_cost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rentals", x => x.id);
                    table.ForeignKey(
                        name: "fk_rentals_couriers_courier_id",
                        column: x => x.courier_id,
                        principalTable: "couriers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_rentals_motorcycles_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "motorcycles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_rentals_courier_id",
                table: "rentals",
                column: "courier_id",
                unique: true,
                filter: "return_date IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_rentals_motorcycle_id",
                table: "rentals",
                column: "motorcycle_id",
                unique: true,
                filter: "return_date IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentals");
        }
    }
}
