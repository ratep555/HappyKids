using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class DiscountCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_childrenItemDiscounts_ChildrenItems_ChildrenItemId",
                table: "childrenItemDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_childrenItemDiscounts_Discounts_DiscountId",
                table: "childrenItemDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_childrenItemDiscounts",
                table: "childrenItemDiscounts");

            migrationBuilder.RenameTable(
                name: "childrenItemDiscounts",
                newName: "ChildrenItemDiscounts");

            migrationBuilder.RenameIndex(
                name: "IX_childrenItemDiscounts_DiscountId",
                table: "ChildrenItemDiscounts",
                newName: "IX_ChildrenItemDiscounts_DiscountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChildrenItemDiscounts",
                table: "ChildrenItemDiscounts",
                columns: new[] { "ChildrenItemId", "DiscountId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenItemDiscounts_ChildrenItems_ChildrenItemId",
                table: "ChildrenItemDiscounts",
                column: "ChildrenItemId",
                principalTable: "ChildrenItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildrenItemDiscounts_Discounts_DiscountId",
                table: "ChildrenItemDiscounts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenItemDiscounts_ChildrenItems_ChildrenItemId",
                table: "ChildrenItemDiscounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildrenItemDiscounts_Discounts_DiscountId",
                table: "ChildrenItemDiscounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChildrenItemDiscounts",
                table: "ChildrenItemDiscounts");

            migrationBuilder.RenameTable(
                name: "ChildrenItemDiscounts",
                newName: "childrenItemDiscounts");

            migrationBuilder.RenameIndex(
                name: "IX_ChildrenItemDiscounts_DiscountId",
                table: "childrenItemDiscounts",
                newName: "IX_childrenItemDiscounts_DiscountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_childrenItemDiscounts",
                table: "childrenItemDiscounts",
                columns: new[] { "ChildrenItemId", "DiscountId" });

            migrationBuilder.AddForeignKey(
                name: "FK_childrenItemDiscounts_ChildrenItems_ChildrenItemId",
                table: "childrenItemDiscounts",
                column: "ChildrenItemId",
                principalTable: "ChildrenItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_childrenItemDiscounts_Discounts_DiscountId",
                table: "childrenItemDiscounts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
