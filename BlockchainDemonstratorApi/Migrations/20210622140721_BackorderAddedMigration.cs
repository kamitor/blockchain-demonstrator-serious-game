using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class BackorderAddedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "farmerTransport",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "manuTransport",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "procTransport",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "retailTransport",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "Backorder",
                table: "Players",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Backorder",
                table: "Players");

            migrationBuilder.AddColumn<int>(
                name: "farmerTransport",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "manuTransport",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "procTransport",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "retailTransport",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
