using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_DeliveryToPlayerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_RequestForPlayerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Order_CurrentOrderId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_RequestForPlayerId",
                table: "Orders",
                newName: "IX_Orders_RequestForPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DeliveryToPlayerId",
                table: "Orders",
                newName: "IX_Orders_DeliveryToPlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_DeliveryToPlayerId",
                table: "Orders",
                column: "DeliveryToPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_RequestForPlayerId",
                table: "Orders",
                column: "RequestForPlayerId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_DeliveryToPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_RequestForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Orders_CurrentOrderId",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_RequestForPlayerId",
                table: "Order",
                newName: "IX_Order_RequestForPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryToPlayerId",
                table: "Order",
                newName: "IX_Order_DeliveryToPlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Order_CurrentOrderId",
                table: "Player",
                column: "CurrentOrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
