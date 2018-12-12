using Microsoft.EntityFrameworkCore.Migrations;

namespace CareSys_API.Migrations
{
    public partial class Intial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CareHomes",
                table: "CareHomes");

            migrationBuilder.RenameTable(
                name: "CareHomes",
                newName: "CareHome");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CareHome",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CareHome",
                table: "CareHome",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CareHome",
                table: "CareHome");

            migrationBuilder.RenameTable(
                name: "CareHome",
                newName: "CareHomes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CareHomes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CareHomes",
                table: "CareHomes",
                column: "Id");
        }
    }
}
