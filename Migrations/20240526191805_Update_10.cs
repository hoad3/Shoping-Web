using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemShoping_CartShoping_CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping");

            migrationBuilder.DropIndex(
                name: "IX_CartItemShoping_CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping");

            migrationBuilder.DropColumn(
                name: "CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemShoping_CartId",
                schema: "Data",
                table: "CartItemShoping",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemShoping_CartShoping_CartId",
                schema: "Data",
                table: "CartItemShoping",
                column: "CartId",
                principalSchema: "Data",
                principalTable: "CartShoping",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemShoping_CartShoping_CartId",
                schema: "Data",
                table: "CartItemShoping");

            migrationBuilder.DropIndex(
                name: "IX_CartItemShoping_CartId",
                schema: "Data",
                table: "CartItemShoping");

            migrationBuilder.AddColumn<int>(
                name: "CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItemShoping_CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping",
                column: "CartShopingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemShoping_CartShoping_CartShopingCartId",
                schema: "Data",
                table: "CartItemShoping",
                column: "CartShopingCartId",
                principalSchema: "Data",
                principalTable: "CartShoping",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
