using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistences.Migrations
{
    /// <inheritdoc />
    public partial class check3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "dbo",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Products");
        }
    }
}
