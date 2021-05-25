using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class ExtraFactorsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "defaultInventory",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "orderLeadTimeRandomMaximum",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "orderLeadTimeRandomMinimum",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "retailerOrderVolumeRandomMaximum",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "retailerOrderVolumeRandomMinimum",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "setUpDeliveryVolume",
                table: "Factors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "setUpOrderVolume",
                table: "Factors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "defaultInventory",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "orderLeadTimeRandomMaximum",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "orderLeadTimeRandomMinimum",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "retailerOrderVolumeRandomMaximum",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "retailerOrderVolumeRandomMinimum",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "setUpDeliveryVolume",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "setUpOrderVolume",
                table: "Factors");
        }
    }
}
