using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class RoleFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Role_Option_DltName",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Option_TrustedPartyName",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Option_YouProvideName",
                table: "Role");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Option_YouProvideWithHelpName",
                table: "Role");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Role_DltName",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_TrustedPartyName",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_YouProvideName",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Role_YouProvideWithHelpName",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "DltName",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "TrustedPartyName",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "YouProvideName",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "YouProvideWithHelpName",
                table: "Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DltName",
                table: "Role",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrustedPartyName",
                table: "Role",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouProvideName",
                table: "Role",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouProvideWithHelpName",
                table: "Role",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CostOfMaintenance = table.Column<int>(type: "int", nullable: false),
                    CostOfStartUp = table.Column<int>(type: "int", nullable: false),
                    Flexibility = table.Column<int>(type: "int", nullable: false),
                    GuaranteedCapacity = table.Column<int>(type: "int", nullable: false),
                    LeadTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_DltName",
                table: "Role",
                column: "DltName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TrustedPartyName",
                table: "Role",
                column: "TrustedPartyName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_YouProvideName",
                table: "Role",
                column: "YouProvideName");

            migrationBuilder.CreateIndex(
                name: "IX_Role_YouProvideWithHelpName",
                table: "Role",
                column: "YouProvideWithHelpName");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Option_DltName",
                table: "Role",
                column: "DltName",
                principalTable: "Option",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Option_TrustedPartyName",
                table: "Role",
                column: "TrustedPartyName",
                principalTable: "Option",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Option_YouProvideName",
                table: "Role",
                column: "YouProvideName",
                principalTable: "Option",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Option_YouProvideWithHelpName",
                table: "Role",
                column: "YouProvideWithHelpName",
                principalTable: "Option",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
