using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class FactorAndPaymentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToPlayer",
                table: "Payment");

            migrationBuilder.AddColumn<bool>(
                name: "FromPlayer",
                table: "Payment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GameStarted",
                table: "Games",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    retailTransport = table.Column<int>(nullable: false),
                    manuTransport = table.Column<int>(nullable: false),
                    procTransport = table.Column<int>(nullable: false),
                    farmerTransport = table.Column<int>(nullable: false),
                    holdingFactor = table.Column<int>(nullable: false),
                    roundIncrement = table.Column<int>(nullable: false),
                    retailProductPrice = table.Column<int>(nullable: false),
                    manuProductPrice = table.Column<int>(nullable: false),
                    procProductPrice = table.Column<int>(nullable: false),
                    farmerProductPrice = table.Column<int>(nullable: false),
                    harvesterProductPrice = table.Column<int>(nullable: false),
                    setupCost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropColumn(
                name: "FromPlayer",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "GameStarted",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "ToPlayer",
                table: "Payment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
