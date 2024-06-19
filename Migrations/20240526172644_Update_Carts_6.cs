using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_Carts_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Data",
                table: "Carts",
                newName: "Userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Userid",
                schema: "Data",
                table: "Carts",
                newName: "UserId");
        }
    }
}
