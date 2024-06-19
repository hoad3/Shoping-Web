using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartShoping",
                schema: "Data",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartShoping", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "ItemCart",
                schema: "Data",
                columns: table => new
                {
                    ItemCartId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: false),
                    CartShopingCartId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCart", x => x.ItemCartId);
                    table.ForeignKey(
                        name: "FK_ItemCart_CartShoping_CartShopingCartId",
                        column: x => x.CartShopingCartId,
                        principalSchema: "Data",
                        principalTable: "CartShoping",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCart_product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Data",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCart_CartShopingCartId",
                schema: "Data",
                table: "ItemCart",
                column: "CartShopingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCart_ProductId",
                schema: "Data",
                table: "ItemCart",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCart",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "CartShoping",
                schema: "Data");
        }
    }
}
