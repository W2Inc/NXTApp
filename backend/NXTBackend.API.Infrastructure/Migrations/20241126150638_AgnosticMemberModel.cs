using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgnosticMemberModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_goal_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.RenameColumn(
                name: "user_goal_id",
                table: "tbl_user_project_member",
                newName: "UserGoalId");

            migrationBuilder.AlterColumn<Guid>(
                name: "git_info_id",
                table: "tbl_user_project",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_project_member_UserGoalId",
                table: "tbl_user_project_member",
                column: "UserGoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_goal_UserGoalId",
                table: "tbl_user_project_member",
                column: "UserGoalId",
                principalTable: "tbl_user_goal",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_goal_UserGoalId",
                table: "tbl_user_project_member");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_project_member_UserGoalId",
                table: "tbl_user_project_member");

            migrationBuilder.RenameColumn(
                name: "UserGoalId",
                table: "tbl_user_project_member",
                newName: "user_goal_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "git_info_id",
                table: "tbl_user_project",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_goal_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
