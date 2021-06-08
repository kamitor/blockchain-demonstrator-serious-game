using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class GameMasterAdminMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameMasters",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    GameId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameMasters_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameMasters_GameId",
                table: "GameMasters",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "GameMasters");

            migrationBuilder.AddColumn<double>(
                name: "ProductPrice",
                table: "Roles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
