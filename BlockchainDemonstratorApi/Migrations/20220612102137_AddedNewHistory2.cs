using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class AddedNewHistory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceivedOrderHistoryJson",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentOrderHistoryJson",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedOrderHistoryJson",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "SentOrderHistoryJson",
                table: "Players");
        }
    }
}
