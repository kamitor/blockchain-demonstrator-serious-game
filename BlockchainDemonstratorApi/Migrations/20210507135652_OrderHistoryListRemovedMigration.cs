using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class OrderHistoryListRemovedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delivery_Orders_OrderId",
                table: "Delivery");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_PlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PlayerId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Delivery",
                table: "Delivery");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Delivery",
                newName: "Deliveries");

            migrationBuilder.RenameIndex(
                name: "IX_Delivery_OrderId",
                table: "Deliveries",
                newName: "IX_Deliveries_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deliveries",
                table: "Deliveries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Orders_OrderId",
                table: "Deliveries",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Orders_OrderId",
                table: "Deliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deliveries",
                table: "Deliveries");

            migrationBuilder.RenameTable(
                name: "Deliveries",
                newName: "Delivery");

            migrationBuilder.RenameIndex(
                name: "IX_Deliveries_OrderId",
                table: "Delivery",
                newName: "IX_Delivery_OrderId");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Delivery",
                table: "Delivery",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlayerId",
                table: "Orders",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delivery_Orders_OrderId",
                table: "Delivery",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_PlayerId",
                table: "Orders",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
