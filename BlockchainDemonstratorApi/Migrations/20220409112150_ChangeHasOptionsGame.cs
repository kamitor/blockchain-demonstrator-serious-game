using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class ChangeHasOptionsGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "noOptions",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "HasOptions",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasOptions",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "noOptions",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
