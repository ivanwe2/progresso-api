using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipBetweenKeywordAndKeywordSinglePlayerResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KeywordSinglePlayerResults_KeywordId",
                table: "KeywordSinglePlayerResults",
                column: "KeywordId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeywordSinglePlayerResults_Keywords_KeywordId",
                table: "KeywordSinglePlayerResults",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordSinglePlayerResults_Keywords_KeywordId",
                table: "KeywordSinglePlayerResults");

            migrationBuilder.DropIndex(
                name: "IX_KeywordSinglePlayerResults_KeywordId",
                table: "KeywordSinglePlayerResults");
        }
    }
}
