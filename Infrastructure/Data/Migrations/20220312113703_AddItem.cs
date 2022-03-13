using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class AddItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumOrderValue",
                table: "Discounts",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "childrenItemDiscounts",
                columns: table => new
                {
                    ChildrenItemId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_childrenItemDiscounts", x => new { x.ChildrenItemId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_childrenItemDiscounts_ChildrenItems_ChildrenItemId",
                        column: x => x.ChildrenItemId,
                        principalTable: "ChildrenItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_childrenItemDiscounts_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChildrenItemWarehouses",
                columns: table => new
                {
                    ChildrenItemId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildrenItemWarehouses", x => new { x.ChildrenItemId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_ChildrenItemWarehouses_ChildrenItems_ChildrenItemId",
                        column: x => x.ChildrenItemId,
                        principalTable: "ChildrenItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildrenItemWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_childrenItemDiscounts_DiscountId",
                table: "childrenItemDiscounts",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenItemWarehouses_WarehouseId",
                table: "ChildrenItemWarehouses",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "childrenItemDiscounts");

            migrationBuilder.DropTable(
                name: "ChildrenItemWarehouses");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "MinimumOrderValue",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Discounts");
        }
    }
}
