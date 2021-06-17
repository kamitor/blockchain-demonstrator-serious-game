using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class GameMasterGamesList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameMasters_Games_GameId",
                table: "GameMasters");

            migrationBuilder.DropIndex(
                name: "IX_GameMasters_GameId",
                table: "GameMasters");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameMasters");

            migrationBuilder.AddColumn<string>(
                name: "GameMasterId",
                table: "Games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameMasterId",
                table: "Games",
                column: "GameMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameMasters_GameMasterId",
                table: "Games",
                column: "GameMasterId",
                principalTable: "GameMasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameMasters_GameMasterId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameMasterId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameMasterId",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "GameId",
                table: "GameMasters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameMasters_GameId",
                table: "GameMasters",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameMasters_Games_GameId",
                table: "GameMasters",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
