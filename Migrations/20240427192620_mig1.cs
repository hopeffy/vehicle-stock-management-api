using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vehicle_stock_management_api.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    plaka = table.Column<string>(type: "text", nullable: false),
                    modelYear = table.Column<int>(type: "integer", nullable: false),
                    muayeneTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
