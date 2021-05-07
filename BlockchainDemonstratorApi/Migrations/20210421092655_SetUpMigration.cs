using Microsoft.EntityFrameworkCore.Migrations;

namespace BlockchainDemonstratorApi.Migrations
{
    public partial class SetUpMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Inventory = table.Column<int>(nullable: false),
                    Backorder = table.Column<int>(nullable: false),
                    IncomingOrderId = table.Column<string>(nullable: true),
                    CurrentOrderId = table.Column<string>(nullable: true),
                    Money = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CurrentPhase = table.Column<int>(nullable: false),
                    CurrentDay = table.Column<int>(nullable: false),
                    RetailerId = table.Column<string>(nullable: true),
                    ManufacturerId = table.Column<string>(nullable: true),
                    ProcessorId = table.Column<string>(nullable: true),
                    FarmerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Player_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Player_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Player_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Player_RetailerId",
                        column: x => x.RetailerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OrderDay = table.Column<int>(nullable: false),
                    ArrivalDay = table.Column<double>(nullable: false),
                    Volume = table.Column<int>(nullable: false),
                    PlayerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_FarmerId",
                table: "Game",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ManufacturerId",
                table: "Game",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ProcessorId",
                table: "Game",
                column: "ProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_RetailerId",
                table: "Game",
                column: "RetailerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlayerId",
                table: "Order",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_CurrentOrderId",
                table: "Player",
                column: "CurrentOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_IncomingOrderId",
                table: "Player",
                column: "IncomingOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Order_CurrentOrderId",
                table: "Player",
                column: "CurrentOrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Order_IncomingOrderId",
                table: "Player",
                column: "IncomingOrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Player_PlayerId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
