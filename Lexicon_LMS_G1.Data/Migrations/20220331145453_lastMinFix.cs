using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicon_LMS_G1.Data.Migrations
{
    public partial class lastMinFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StudentDocuments_ActivityId",
                table: "StudentDocuments",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentDocuments_Activities_ActivityId",
                table: "StudentDocuments",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentDocuments_Activities_ActivityId",
                table: "StudentDocuments");

            migrationBuilder.DropIndex(
                name: "IX_StudentDocuments_ActivityId",
                table: "StudentDocuments");
        }
    }
}
