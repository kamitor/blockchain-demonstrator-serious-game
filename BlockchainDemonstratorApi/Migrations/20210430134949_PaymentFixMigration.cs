using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class PaymentFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Payment");

            migrationBuilder.AddColumn<bool>(
                name: "ToPlayer",
                table: "Payment",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToPlayer",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
