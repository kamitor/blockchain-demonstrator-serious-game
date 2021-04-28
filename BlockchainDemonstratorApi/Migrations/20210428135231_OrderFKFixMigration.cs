using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderFKFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_DeliverToPlayerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_OrderFromPlayerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeliverToPlayerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderFromPlayerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliverToPlayerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderFromPlayerId",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryToPlayerId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestForPlayerId",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliveryToPlayerId",
                table: "Order",
                column: "DeliveryToPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RequestForPlayerId",
                table: "Order",
                column: "RequestForPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_DeliveryToPlayerId",
                table: "Order",
                column: "DeliveryToPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_RequestForPlayerId",
                table: "Order",
                column: "RequestForPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_DeliveryToPlayerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_RequestForPlayerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DeliveryToPlayerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_RequestForPlayerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryToPlayerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "RequestForPlayerId",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "DeliverToPlayerId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderFromPlayerId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_DeliverToPlayerId",
                table: "Order",
                column: "DeliverToPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderFromPlayerId",
                table: "Order",
                column: "OrderFromPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_DeliverToPlayerId",
                table: "Order",
                column: "DeliverToPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_OrderFromPlayerId",
                table: "Order",
                column: "OrderFromPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
