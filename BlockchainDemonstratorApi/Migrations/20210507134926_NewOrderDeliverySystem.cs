using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class NewOrderDeliverySystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_DeliveryToPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_HistoryOfPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_RequestForPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryToPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_HistoryOfPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RequestForPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ArrivalDay",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryToPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HistoryOfPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RequestForPlayerId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "IncomingOrderForPlayerId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingOrderForPlayerId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrderId = table.Column<string>(nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    SendDeliveryDay = table.Column<int>(nullable: false),
                    ArrivalDay = table.Column<double>(nullable: false),
                    Processed = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Delivery_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IncomingOrderForPlayerId",
                table: "Orders",
                column: "IncomingOrderForPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OutgoingOrderForPlayerId",
                table: "Orders",
                column: "OutgoingOrderForPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PlayerId",
                table: "Orders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderId",
                table: "Delivery",
                column: "OrderId");

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
                name: "FK_Orders_Player_PlayerId",
                table: "Orders",
                column: "PlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_IncomingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_OutgoingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Player_PlayerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IncomingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OutgoingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IncomingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OutgoingOrderForPlayerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "ArrivalDay",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryToPlayerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HistoryOfPlayerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RequestForPlayerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryToPlayerId",
                table: "Orders",
                column: "DeliveryToPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_HistoryOfPlayerId",
                table: "Orders",
                column: "HistoryOfPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequestForPlayerId",
                table: "Orders",
                column: "RequestForPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_DeliveryToPlayerId",
                table: "Orders",
                column: "DeliveryToPlayerId",
                principalTable: "Player",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Player_HistoryOfPlayerId",
                table: "Orders",
                column: "HistoryOfPlayerId",
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
        }
    }
}
