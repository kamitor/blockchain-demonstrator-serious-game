using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class CamelCasingHistoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "overallProfitHistoryJson",
                table: "Players",
                newName: "OverallProfitHistoryJson");

            migrationBuilder.RenameColumn(
                name: "orderWorthHistoryJson",
                table: "Players",
                newName: "OrderWorthHistoryJson");

            migrationBuilder.RenameColumn(
                name: "inventoryHistoryJson",
                table: "Players",
                newName: "InventoryHistoryJson");

            migrationBuilder.RenameColumn(
                name: "grossProfitHistoryJson",
                table: "Players",
                newName: "GrossProfitHistoryJson");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OverallProfitHistoryJson",
                table: "Players",
                newName: "overallProfitHistoryJson");

            migrationBuilder.RenameColumn(
                name: "OrderWorthHistoryJson",
                table: "Players",
                newName: "orderWorthHistoryJson");

            migrationBuilder.RenameColumn(
                name: "InventoryHistoryJson",
                table: "Players",
                newName: "inventoryHistoryJson");

            migrationBuilder.RenameColumn(
                name: "GrossProfitHistoryJson",
                table: "Players",
                newName: "grossProfitHistoryJson");
        }
    }
}
