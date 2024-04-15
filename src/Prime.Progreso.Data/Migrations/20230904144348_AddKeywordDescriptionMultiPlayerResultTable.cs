using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordDescriptionMultiPlayerResultTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeywordDescriptionMultiPlayerResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KeywordDescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordDescriptionMultiPlayerResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordDescriptionMultiPlayerResults_KeywordDescriptions_KeywordDescriptionId",
                        column: x => x.KeywordDescriptionId,
                        principalTable: "KeywordDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeywordDescriptionMultiPlayerResults_KeywordDescriptionId",
                table: "KeywordDescriptionMultiPlayerResults",
                column: "KeywordDescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeywordDescriptionMultiPlayerResults");
        }
    }
}
