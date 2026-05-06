using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistences.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teachers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentTeacher",
                newName: "StudentTeacher",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Students",
                newSchema: "dbo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "dbo",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "StudentTeacher",
                schema: "dbo",
                newName: "StudentTeacher");

            migrationBuilder.RenameTable(
                name: "Students",
                schema: "dbo",
                newName: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
