using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class RolesAndOptionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Player",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CostOfStartUp = table.Column<int>(nullable: false),
                    CostOfMaintenance = table.Column<int>(nullable: false),
                    LeadTime = table.Column<int>(nullable: false),
                    Flexibility = table.Column<int>(nullable: false),
                    GuaranteedCapacity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    LeadTime = table.Column<double>(nullable: false),
                    YouProvideName = table.Column<string>(nullable: true),
                    YouProvideWithHelpName = table.Column<string>(nullable: true),
                    TrustedPartyName = table.Column<string>(nullable: true),
                    DltName = table.Column<string>(nullable: true),
                    Product = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Role_Option_DltName",
                        column: x => x.DltName,
                        principalTable: "Option",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Option_TrustedPartyName",
                        column: x => x.TrustedPartyName,
                        principalTable: "Option",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Option_YouProvideName",
                        column: x => x.YouProvideName,
                        principalTable: "Option",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Option_YouProvideWithHelpName",
                        column: x => x.YouProvideWithHelpName,
                        principalTable: "Option",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoleName",
                table: "Player",
                column: "RoleName");

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
                name: "FK_Player_Role_RoleName",
                table: "Player",
                column: "RoleName",
                principalTable: "Role",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Role_RoleName",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoleName",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Player");
        }
    }
}
