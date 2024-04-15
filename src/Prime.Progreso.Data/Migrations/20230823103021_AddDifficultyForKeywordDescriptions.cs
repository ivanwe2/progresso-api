using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyForKeywordDescriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "KeywordDescriptions",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "KeywordDescriptions");
        }
    }
}
