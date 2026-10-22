using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentBikeApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCouriers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "couriers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    license_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    license_type = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    license_image_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_couriers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_couriers_cnpj",
                table: "couriers",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_couriers_license_number",
                table: "couriers",
                column: "license_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "couriers");
        }
    }
}
