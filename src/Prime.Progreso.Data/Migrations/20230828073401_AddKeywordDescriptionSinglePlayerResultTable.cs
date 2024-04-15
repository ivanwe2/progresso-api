using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordDescriptionSinglePlayerResultTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeywordDescriptionSinglePlayerResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KeywordDescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordDescriptionSinglePlayerResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordDescriptionSinglePlayerResults_KeywordDescriptions_KeywordDescriptionId",
                        column: x => x.KeywordDescriptionId,
                        principalTable: "KeywordDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeywordDescriptionSinglePlayerResults_KeywordDescriptionId",
                table: "KeywordDescriptionSinglePlayerResults",
                column: "KeywordDescriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeywordDescriptionSinglePlayerResults");
        }
    }
}
