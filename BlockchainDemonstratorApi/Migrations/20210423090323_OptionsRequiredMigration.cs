using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OptionsRequiredMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Player_FarmerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Player_ManufacturerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Player_ProcessorId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Player_RetailerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Option_Role_RoleId",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_Option_RoleId",
                table: "Options",
                newName: "IX_Options_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_RetailerId",
                table: "Games",
                newName: "IX_Games_RetailerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_ProcessorId",
                table: "Games",
                newName: "IX_Games_ProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_ManufacturerId",
                table: "Games",
                newName: "IX_Games_ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_FarmerId",
                table: "Games",
                newName: "IX_Games_FarmerId");

            migrationBuilder.AlterColumn<double>(
                name: "LeadTime",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "GuaranteedCapacity",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Flexibility",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CostOfStartUp",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CostOfMaintenance",
                table: "Options",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_FarmerId",
                table: "Games",
                column: "FarmerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_ManufacturerId",
                table: "Games",
                column: "ManufacturerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_ProcessorId",
                table: "Games",
                column: "ProcessorId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Player_RetailerId",
                table: "Games",
                column: "RetailerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Roles_RoleId",
                table: "Options",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Roles_RoleId",
                table: "Player",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_FarmerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_ManufacturerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_ProcessorId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Player_RetailerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Options_Roles_RoleId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Roles_RoleId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameIndex(
                name: "IX_Options_RoleId",
                table: "Option",
                newName: "IX_Option_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_RetailerId",
                table: "Game",
                newName: "IX_Game_RetailerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_ProcessorId",
                table: "Game",
                newName: "IX_Game_ProcessorId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_ManufacturerId",
                table: "Game",
                newName: "IX_Game_ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_FarmerId",
                table: "Game",
                newName: "IX_Game_FarmerId");

            migrationBuilder.AlterColumn<int>(
                name: "LeadTime",
                table: "Option",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "GuaranteedCapacity",
                table: "Option",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Flexibility",
                table: "Option",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CostOfStartUp",
                table: "Option",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "CostOfMaintenance",
                table: "Option",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Player_FarmerId",
                table: "Game",
                column: "FarmerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Player_ManufacturerId",
                table: "Game",
                column: "ManufacturerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Player_ProcessorId",
                table: "Game",
                column: "ProcessorId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Player_RetailerId",
                table: "Game",
                column: "RetailerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Role_RoleId",
                table: "Option",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Role_RoleId",
                table: "Player",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
