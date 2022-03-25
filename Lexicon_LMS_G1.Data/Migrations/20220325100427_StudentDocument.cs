using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_LMS_G1.Data.Migrations
{
    public partial class StudentDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "StudentDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "StudentDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "StudentDocuments");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "StudentDocuments");
        }
    }
}
