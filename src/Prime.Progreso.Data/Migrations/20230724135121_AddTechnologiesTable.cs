using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.Progreso.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnologiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumItems_Curriculums_CurriculumId",
                table: "CurriculumItems");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumItems_CurriculumId",
                table: "CurriculumItems");

            migrationBuilder.DropTable(
                name: "CurriculumItems");

            migrationBuilder.DropTable(
                name: "Curriculums");

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Curriculums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curriculums_Technologies_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "Technologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });           

            migrationBuilder.CreateTable(
                name: "CurriculumItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurriculumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfInternship = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurriculumItems_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurriculumItems_Curriculums_CurriculumId",
                        column: x => x.CurriculumId,
                        principalTable: "Curriculums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curriculums_TechnologyId",
                table: "Curriculums",
                column: "TechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumItems_ActivityId",
                table: "CurriculumItems",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CurriculumItems_CurriculumId",
                table: "CurriculumItems",
                column: "CurriculumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurriculumItems_Curriculums_CurriculumId",
                table: "CurriculumItems");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumItems_CurriculumId",
                table: "CurriculumItems");

            migrationBuilder.DropIndex(
                name: "IX_Curriculums_TechnologyId",
                table: "Curriculums");

            migrationBuilder.DropIndex(
                name: "IX_CurriculumItems_ActivityId",
                table: "CurriculumItems");

            migrationBuilder.DropTable(
                name: "CurriculumItems");

            migrationBuilder.DropTable(
                name: "Curriculums");

            migrationBuilder.DropTable(
                name: "Technologies");           
        }
    }
}
