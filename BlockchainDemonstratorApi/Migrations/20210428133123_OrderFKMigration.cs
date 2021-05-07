using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderFKMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_PlayerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_PlayerId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PlayerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PlayerId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Order");

            migrationBuilder.AddColumn<string>(
                name: "DeliverToPlayerId",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderFromPlayerId",
                table: "Order",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "PlayerId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId1",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlayerId",
                table: "Order",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlayerId1",
                table: "Order",
                column: "PlayerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_PlayerId",
                table: "Order",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Player_PlayerId1",
                table: "Order",
                column: "PlayerId1",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
