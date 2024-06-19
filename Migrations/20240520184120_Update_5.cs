using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "daycreate",
                schema: "Data",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "phonenumber",
                schema: "Data",
                table: "USER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "daycreate",
                schema: "Data",
                table: "USER",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "phonenumber",
                schema: "Data",
                table: "USER",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
