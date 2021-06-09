using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class HistoryPropertiesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "grossProfitHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "inventoryHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "orderWorthHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "overallProfitHistoryJson",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "grossProfitHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "inventoryHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "orderWorthHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "overallProfitHistoryJson",
                table: "Players");
        }
    }
}
