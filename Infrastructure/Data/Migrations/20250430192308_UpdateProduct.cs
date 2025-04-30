using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProdcutId",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "ProdcutId",
                table: "Image",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ProdcutId",
                table: "Image",
                newName: "IX_Image_ProductId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductOptions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductOptions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "ProductOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Products_ProductId",
                table: "Image",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Products_ProductId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductOptions");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "ProductOptions");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Image",
                newName: "ProdcutId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ProductId",
                table: "Image",
                newName: "IX_Image_ProdcutId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "ProductOptions",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Products_ProdcutId",
                table: "Image",
                column: "ProdcutId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
