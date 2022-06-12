using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class AddedNewHistory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentOrderHistoryJson",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "SentCurrentOrderHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentOrdersHistoryJson",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentCurrentOrderHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SentOrdersHistoryJson",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "SentOrderHistoryJson",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
