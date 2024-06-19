using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_2.Migrations
{
    /// <inheritdoc />
    public partial class Update_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Daycreated",
                schema: "Data",
                table: "product",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Daycreated",
                schema: "Data",
                table: "product",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
