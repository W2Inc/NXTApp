using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GoalProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningGoalProject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningGoalProject",
                columns: table => new
                {
                    GoalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningGoalProject", x => new { x.GoalsId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_LearningGoalProject_tbl_learning_goal_GoalsId",
                        column: x => x.GoalsId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningGoalProject_tbl_project_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningGoalProject_ProjectsId",
                table: "LearningGoalProject",
                column: "ProjectsId");
        }
    }
}
