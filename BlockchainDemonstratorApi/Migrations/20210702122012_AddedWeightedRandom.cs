using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class AddedWeightedRandom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orderLeadTimeRandomMaximum",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "orderLeadTimeRandomMinimum",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "ratioAChance",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ratioALeadtime",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ratioBChance",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ratioBLeadtime",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ratioCChance",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ratioCLeadtime",
                table: "Factors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ratioAChance",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "ratioALeadtime",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "ratioBChance",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "ratioBLeadtime",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "ratioCChance",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "ratioCLeadtime",
                table: "Factors");

            migrationBuilder.AddColumn<int>(
                name: "orderLeadTimeRandomMaximum",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "orderLeadTimeRandomMinimum",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
