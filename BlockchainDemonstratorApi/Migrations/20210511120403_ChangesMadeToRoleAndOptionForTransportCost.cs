using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class ChangesMadeToRoleAndOptionForTransportCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransportationCosts",
                table: "Options");

            migrationBuilder.AddColumn<double>(
                name: "ProductPrice",
                table: "Roles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransportCostOneTrip",
                table: "Options",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransportCostPerDay",
                table: "Options",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "TransportCostOneTrip",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "TransportCostPerDay",
                table: "Options");

            migrationBuilder.AddColumn<double>(
                name: "TransportationCosts",
                table: "Options",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
