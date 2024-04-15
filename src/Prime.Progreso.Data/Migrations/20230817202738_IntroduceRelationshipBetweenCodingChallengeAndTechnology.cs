using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceRelationshipBetweenCodingChallengeAndTechnology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TechnologyId",
                table: "CodingChallenges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CodingChallenges_TechnologyId",
                table: "CodingChallenges",
                column: "TechnologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CodingChallenges_Technologies_TechnologyId",
                table: "CodingChallenges",
                column: "TechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodingChallenges_Technologies_TechnologyId",
                table: "CodingChallenges");

            migrationBuilder.DropIndex(
                name: "IX_CodingChallenges_TechnologyId",
                table: "CodingChallenges");

            migrationBuilder.DropColumn(
                name: "TechnologyId",
                table: "CodingChallenges");
        }
    }
}
