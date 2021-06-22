using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class RemovedRoleLeadtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadTime",
                table: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "LeadTime",
                table: "Roles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
