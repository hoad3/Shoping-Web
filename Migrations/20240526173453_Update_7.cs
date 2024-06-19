using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartsItem",
                schema: "Data");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "Data",
                columns: table => new
                {
                    Cartid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Userid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Cartid);
                });

            migrationBuilder.CreateTable(
                name: "CartsItem",
                schema: "Data",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cartid = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartsItem", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartsItem_Carts_Cartid",
                        column: x => x.Cartid,
                        principalSchema: "Data",
                        principalTable: "Carts",
                        principalColumn: "Cartid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartsItem_product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Data",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartsItem_Cartid",
                schema: "Data",
                table: "CartsItem",
                column: "Cartid");

            migrationBuilder.CreateIndex(
                name: "IX_CartsItem_ProductId",
                schema: "Data",
                table: "CartsItem",
                column: "ProductId");
        }
    }
}
