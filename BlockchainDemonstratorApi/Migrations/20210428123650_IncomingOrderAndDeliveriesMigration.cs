using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class IncomingOrderAndDeliveriesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Order_IncomingOrderId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_IncomingOrderId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "Backorder",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "IncomingOrderId",
                table: "Player");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId1",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlayerId1",
                table: "Order",
                column: "PlayerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_PlayerId1",
                table: "Order",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_PlayerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PlayerId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "Backorder",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncomingOrderId",
                table: "Player",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_IncomingOrderId",
                table: "Player",
                column: "IncomingOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Order_IncomingOrderId",
                table: "Player",
                column: "IncomingOrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
