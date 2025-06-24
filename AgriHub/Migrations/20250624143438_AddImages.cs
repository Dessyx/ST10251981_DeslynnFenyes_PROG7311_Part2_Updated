using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriHub.Migrations
{
    /// <inheritdoc />
    public partial class AddImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category_Price",
                table: "Products",
                columns: new[] { "Category", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId_ProductionDate",
                table: "Products",
                columns: new[] { "FarmerId", "ProductionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductionDate",
                table: "Products",
                column: "ProductionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_Email",
                table: "Farmers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_Name",
                table: "Farmers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_UserId",
                table: "Farmers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Category",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Category_Price",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId_ProductionDate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Price",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductionDate",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_Email",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_Name",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_UserId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Farmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
