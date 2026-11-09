using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentBikeApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameOpenRentalIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_rentals_motorcycle_id",
                table: "rentals",
                newName: "ix_rentals_one_open_per_motorcycle");

            migrationBuilder.RenameIndex(
                name: "ix_rentals_courier_id",
                table: "rentals",
                newName: "ix_rentals_one_open_per_courier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_rentals_one_open_per_motorcycle",
                table: "rentals",
                newName: "ix_rentals_motorcycle_id");

            migrationBuilder.RenameIndex(
                name: "ix_rentals_one_open_per_courier",
                table: "rentals",
                newName: "ix_rentals_courier_id");
        }
    }
}
