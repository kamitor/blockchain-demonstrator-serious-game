using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderHistoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HistoryOfPlayerId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_HistoryOfPlayerId",
                table: "Orders",
                column: "HistoryOfPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_HistoryOfPlayerId",
                table: "Orders",
                column: "HistoryOfPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_HistoryOfPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_HistoryOfPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HistoryOfPlayerId",
                table: "Orders");
        }
    }
}
