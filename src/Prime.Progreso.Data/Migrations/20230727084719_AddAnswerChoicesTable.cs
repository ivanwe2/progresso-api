using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerChoicesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerChoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizExecutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerChoices_Answers_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnswerChoices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AnswerChoices_QuizExecutions_QuizExecutionId",
                        column: x => x.QuizExecutionId,
                        principalTable: "QuizExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerChoices_ChoiceId",
                table: "AnswerChoices",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerChoices_QuestionId",
                table: "AnswerChoices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerChoices_QuizExecutionId",
                table: "AnswerChoices",
                column: "QuizExecutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerChoices");
        }
    }
}
