using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordsTableAndAlterRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_LanguageId",
                table: "Keywords",
                column: "LanguageId");

            migrationBuilder.DropTable(
                name: "KeywordDescriptions");

            migrationBuilder.CreateTable(
                name: "KeywordDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordDescriptions_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeywordDescriptions_KeywordId",
                table: "KeywordDescriptions",
                column: "KeywordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeywordDescriptions_Keywords_KeywordId",
                table: "KeywordDescriptions");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropIndex(
                name: "IX_KeywordDescriptions_KeywordId",
                table: "KeywordDescriptions");

            migrationBuilder.DropColumn(
                name: "KeywordId",
                table: "KeywordDescriptions");
        }
    }
}
