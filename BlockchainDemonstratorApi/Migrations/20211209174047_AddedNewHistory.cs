using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class AddedNewHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackorderHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BalanceHistoryJson",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackorderHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "BalanceHistoryJson",
                table: "Players");
        }
    }
}
