using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_LMS_G1.Data.Migrations
{
    public partial class DocumentUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ModuleDocuments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "CourseDocuments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "ActivityDocuments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "StudentDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ModuleDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "CourseDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "ActivityDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
