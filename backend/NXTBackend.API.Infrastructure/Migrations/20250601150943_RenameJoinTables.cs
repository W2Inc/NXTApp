using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameJoinTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CursusCollaborator_tbl_cursus_CursusId",
                table: "CursusCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_CursusCollaborator_tbl_user_UserId",
                table: "CursusCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalCollaborator_tbl_learning_goal_GoalId",
                table: "GoalCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalCollaborator_tbl_user_UserId",
                table: "GoalCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalProject_tbl_learning_goal_GoalId",
                table: "GoalProject");

            migrationBuilder.DropForeignKey(
                name: "FK_GoalProject_tbl_project_ProjectId",
                table: "GoalProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoalProject",
                table: "GoalProject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GoalCollaborator",
                table: "GoalCollaborator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CursusCollaborator",
                table: "CursusCollaborator");

            migrationBuilder.RenameTable(
                name: "GoalProject",
                newName: "rel_goalproject");

            migrationBuilder.RenameTable(
                name: "GoalCollaborator",
                newName: "rel_goalcollaborator");

            migrationBuilder.RenameTable(
                name: "CursusCollaborator",
                newName: "rel_CursusCollaborator");

            migrationBuilder.RenameIndex(
                name: "IX_GoalProject_GoalId",
                table: "rel_goalproject",
                newName: "IX_rel_goalproject_GoalId");

            migrationBuilder.RenameIndex(
                name: "IX_GoalCollaborator_GoalId",
                table: "rel_goalcollaborator",
                newName: "IX_rel_goalcollaborator_GoalId");

            migrationBuilder.RenameIndex(
                name: "IX_CursusCollaborator_CursusId",
                table: "rel_CursusCollaborator",
                newName: "IX_rel_CursusCollaborator_CursusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rel_goalproject",
                table: "rel_goalproject",
                columns: new[] { "ProjectId", "GoalId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_rel_goalcollaborator",
                table: "rel_goalcollaborator",
                columns: new[] { "UserId", "GoalId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_rel_CursusCollaborator",
                table: "rel_CursusCollaborator",
                columns: new[] { "UserId", "CursusId" });

            migrationBuilder.AddForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_cursus_CursusId",
                table: "rel_CursusCollaborator",
                column: "CursusId",
                principalTable: "tbl_cursus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_user_UserId",
                table: "rel_CursusCollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_goalcollaborator_tbl_learning_goal_GoalId",
                table: "rel_goalcollaborator",
                column: "GoalId",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_goalcollaborator_tbl_user_UserId",
                table: "rel_goalcollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_goalproject_tbl_learning_goal_GoalId",
                table: "rel_goalproject",
                column: "GoalId",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_goalproject_tbl_project_ProjectId",
                table: "rel_goalproject",
                column: "ProjectId",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_cursus_CursusId",
                table: "rel_CursusCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_user_UserId",
                table: "rel_CursusCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_goalcollaborator_tbl_learning_goal_GoalId",
                table: "rel_goalcollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_goalcollaborator_tbl_user_UserId",
                table: "rel_goalcollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_goalproject_tbl_learning_goal_GoalId",
                table: "rel_goalproject");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_goalproject_tbl_project_ProjectId",
                table: "rel_goalproject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rel_goalproject",
                table: "rel_goalproject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rel_goalcollaborator",
                table: "rel_goalcollaborator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rel_CursusCollaborator",
                table: "rel_CursusCollaborator");

            migrationBuilder.RenameTable(
                name: "rel_goalproject",
                newName: "GoalProject");

            migrationBuilder.RenameTable(
                name: "rel_goalcollaborator",
                newName: "GoalCollaborator");

            migrationBuilder.RenameTable(
                name: "rel_CursusCollaborator",
                newName: "CursusCollaborator");

            migrationBuilder.RenameIndex(
                name: "IX_rel_goalproject_GoalId",
                table: "GoalProject",
                newName: "IX_GoalProject_GoalId");

            migrationBuilder.RenameIndex(
                name: "IX_rel_goalcollaborator_GoalId",
                table: "GoalCollaborator",
                newName: "IX_GoalCollaborator_GoalId");

            migrationBuilder.RenameIndex(
                name: "IX_rel_CursusCollaborator_CursusId",
                table: "CursusCollaborator",
                newName: "IX_CursusCollaborator_CursusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoalProject",
                table: "GoalProject",
                columns: new[] { "ProjectId", "GoalId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GoalCollaborator",
                table: "GoalCollaborator",
                columns: new[] { "UserId", "GoalId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CursusCollaborator",
                table: "CursusCollaborator",
                columns: new[] { "UserId", "CursusId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CursusCollaborator_tbl_cursus_CursusId",
                table: "CursusCollaborator",
                column: "CursusId",
                principalTable: "tbl_cursus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CursusCollaborator_tbl_user_UserId",
                table: "CursusCollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalCollaborator_tbl_learning_goal_GoalId",
                table: "GoalCollaborator",
                column: "GoalId",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalCollaborator_tbl_user_UserId",
                table: "GoalCollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalProject_tbl_learning_goal_GoalId",
                table: "GoalProject",
                column: "GoalId",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GoalProject_tbl_project_ProjectId",
                table: "GoalProject",
                column: "ProjectId",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
