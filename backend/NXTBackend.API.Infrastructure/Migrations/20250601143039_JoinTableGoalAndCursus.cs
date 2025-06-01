using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JoinTableGoalAndCursus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cursus_tbl_user_creator_id",
                table: "tbl_cursus");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_learning_goal_tbl_user_creator_id",
                table: "tbl_learning_goal");

            migrationBuilder.DropTable(
                name: "CollaboratorsOnCursi");

            migrationBuilder.DropTable(
                name: "CollaboratorsOnGoals");

            migrationBuilder.CreateTable(
                name: "CursusCollaborator",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CursusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursusCollaborator", x => new { x.UserId, x.CursusId });
                    table.ForeignKey(
                        name: "FK_CursusCollaborator_tbl_cursus_CursusId",
                        column: x => x.CursusId,
                        principalTable: "tbl_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursusCollaborator_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalCollaborator",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoalId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalCollaborator", x => new { x.UserId, x.GoalId });
                    table.ForeignKey(
                        name: "FK_GoalCollaborator_tbl_learning_goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoalCollaborator_tbl_user_UserId",
                        column: x => x.UserId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalProject",
                columns: table => new
                {
                    GoalId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalProject", x => new { x.ProjectId, x.GoalId });
                    table.ForeignKey(
                        name: "FK_GoalProject_tbl_learning_goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoalProject_tbl_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "tbl_project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CursusCollaborator_CursusId",
                table: "CursusCollaborator",
                column: "CursusId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalCollaborator_GoalId",
                table: "GoalCollaborator",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalProject_GoalId",
                table: "GoalProject",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cursus_tbl_user_creator_id",
                table: "tbl_cursus",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_learning_goal_tbl_user_creator_id",
                table: "tbl_learning_goal",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_cursus_tbl_user_creator_id",
                table: "tbl_cursus");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_learning_goal_tbl_user_creator_id",
                table: "tbl_learning_goal");

            migrationBuilder.DropTable(
                name: "CursusCollaborator");

            migrationBuilder.DropTable(
                name: "GoalCollaborator");

            migrationBuilder.DropTable(
                name: "GoalProject");

            migrationBuilder.CreateTable(
                name: "CollaboratorsOnCursi",
                columns: table => new
                {
                    CollaboratedCursiId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorsOnCursi", x => new { x.CollaboratedCursiId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnCursi_tbl_cursus_CollaboratedCursiId",
                        column: x => x.CollaboratedCursiId,
                        principalTable: "tbl_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnCursi_tbl_user_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollaboratorsOnGoals",
                columns: table => new
                {
                    CollaboratedGoalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorsOnGoals", x => new { x.CollaboratedGoalsId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnGoals_tbl_learning_goal_CollaboratedGoalsId",
                        column: x => x.CollaboratedGoalsId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnGoals_tbl_user_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorsOnCursi_CollaboratorsId",
                table: "CollaboratorsOnCursi",
                column: "CollaboratorsId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorsOnGoals_CollaboratorsId",
                table: "CollaboratorsOnGoals",
                column: "CollaboratorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_cursus_tbl_user_creator_id",
                table: "tbl_cursus",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_learning_goal_tbl_user_creator_id",
                table: "tbl_learning_goal",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
