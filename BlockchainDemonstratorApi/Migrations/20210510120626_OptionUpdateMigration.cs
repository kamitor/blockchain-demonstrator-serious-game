using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OptionUpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuaranteedCapacity",
                table: "Options");

            migrationBuilder.AddColumn<double>(
                name: "GuaranteedCapacityPenalty",
                table: "Options",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransportationCosts",
                table: "Options",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuaranteedCapacityPenalty",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "TransportationCosts",
                table: "Options");

            migrationBuilder.AddColumn<double>(
                name: "GuaranteedCapacity",
                table: "Options",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
