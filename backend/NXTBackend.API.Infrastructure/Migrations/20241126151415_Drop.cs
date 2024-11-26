using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Drop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_goal_UserGoalId",
                table: "tbl_user_project_member");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_project_member_UserGoalId",
                table: "tbl_user_project_member");

            migrationBuilder.DropColumn(
                name: "UserGoalId",
                table: "tbl_user_project_member");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserGoalId",
                table: "tbl_user_project_member",
                type: "uuid",
                nullable: true);

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
    }
}
