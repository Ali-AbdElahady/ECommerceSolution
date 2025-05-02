using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOptionToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductOptionId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductOptionId",
                table: "OrderItems",
                column: "ProductOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductOptions_ProductOptionId",
                table: "OrderItems",
                column: "ProductOptionId",
                principalTable: "ProductOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductOptions_ProductOptionId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductOptionId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductOptionId",
                table: "OrderItems");
        }
    }
}
