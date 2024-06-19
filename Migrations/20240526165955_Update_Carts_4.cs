using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_Carts_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItem_Carts_CartId",
                schema: "Data",
                table: "CartsItem");

            migrationBuilder.RenameColumn(
                name: "CartId",
                schema: "Data",
                table: "CartsItem",
                newName: "Cartid");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItem_CartId",
                schema: "Data",
                table: "CartsItem",
                newName: "IX_CartsItem_Cartid");

            migrationBuilder.RenameColumn(
                name: "CartId",
                schema: "Data",
                table: "Carts",
                newName: "Cartid");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItem_Carts_Cartid",
                schema: "Data",
                table: "CartsItem",
                column: "Cartid",
                principalSchema: "Data",
                principalTable: "Carts",
                principalColumn: "Cartid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartsItem_Carts_Cartid",
                schema: "Data",
                table: "CartsItem");

            migrationBuilder.RenameColumn(
                name: "Cartid",
                schema: "Data",
                table: "CartsItem",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartsItem_Cartid",
                schema: "Data",
                table: "CartsItem",
                newName: "IX_CartsItem_CartId");

            migrationBuilder.RenameColumn(
                name: "Cartid",
                schema: "Data",
                table: "Carts",
                newName: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartsItem_Carts_CartId",
                schema: "Data",
                table: "CartsItem",
                column: "CartId",
                principalSchema: "Data",
                principalTable: "Carts",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
