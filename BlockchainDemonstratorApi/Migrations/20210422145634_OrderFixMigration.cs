using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Role_RoleName",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoleName",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Player");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Role",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Player",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CostOfStartUp = table.Column<int>(nullable: false),
                    CostOfMaintenance = table.Column<int>(nullable: false),
                    LeadTime = table.Column<int>(nullable: false),
                    Flexibility = table.Column<int>(nullable: false),
                    GuaranteedCapacity = table.Column<int>(nullable: false),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoleId",
                table: "Player",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Option_RoleId",
                table: "Option",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Player_RoleId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Player");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Role",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Player",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Player_RoleName",
                table: "Player",
                column: "RoleName");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Role_RoleName",
                table: "Player",
                column: "RoleName",
                principalTable: "Role",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
