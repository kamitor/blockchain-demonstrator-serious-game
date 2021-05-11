using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class AddedPlayersAndPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_Orders_Player_IncomingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_OutgoingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Player_PlayerId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Orders_CurrentOrderId",
                table: "Player");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Roles_RoleId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Player_RoleId",
                table: "Players",
                newName: "IX_Players_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Player_CurrentOrderId",
                table: "Players",
                newName: "IX_Players_CurrentOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_PlayerId",
                table: "Payments",
                newName: "IX_Payments_PlayerId");

            migrationBuilder.AddColumn<string>(
                name: "ChosenOptionId",
                table: "Players",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ChosenOptionId",
                table: "Players",
                column: "ChosenOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_FarmerId",
                table: "Games",
                column: "FarmerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_ManufacturerId",
                table: "Games",
                column: "ManufacturerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_ProcessorId",
                table: "Games",
                column: "ProcessorId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_RetailerId",
                table: "Games",
                column: "RetailerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Players_IncomingOrderForPlayerId",
                table: "Orders",
                column: "IncomingOrderForPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Players_OutgoingOrderForPlayerId",
                table: "Orders",
                column: "OutgoingOrderForPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Players_PlayerId",
                table: "Payments",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Options_ChosenOptionId",
                table: "Players",
                column: "ChosenOptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Orders_CurrentOrderId",
                table: "Players",
                column: "CurrentOrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Roles_RoleId",
                table: "Players",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_FarmerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_ManufacturerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_ProcessorId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_RetailerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Players_IncomingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Players_OutgoingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Players_PlayerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Options_ChosenOptionId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Orders_CurrentOrderId",
                table: "Players");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Roles_RoleId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_ChosenOptionId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ChosenOptionId",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Players_RoleId",
                table: "Player",
                newName: "IX_Player_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Players_CurrentOrderId",
                table: "Player",
                newName: "IX_Player_CurrentOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_PlayerId",
                table: "Payment",
                newName: "IX_Payment_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
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
                name: "FK_Orders_Player_IncomingOrderForPlayerId",
                table: "Orders",
                column: "IncomingOrderForPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_OutgoingOrderForPlayerId",
                table: "Orders",
                column: "OutgoingOrderForPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Player_PlayerId",
                table: "Payment",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Orders_CurrentOrderId",
                table: "Player",
                column: "CurrentOrderId",
                principalTable: "Orders",
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
    }
}
