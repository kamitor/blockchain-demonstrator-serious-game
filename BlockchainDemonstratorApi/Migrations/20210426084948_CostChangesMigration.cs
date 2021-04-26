using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class CostChangesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Money",
                table: "Player");

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Player",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ItemPrice",
                table: "Player",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Order",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Order");

            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Player",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
