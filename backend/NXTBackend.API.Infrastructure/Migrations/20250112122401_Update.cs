using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_user_reviewer_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_display_name",
                table: "tbl_user",
                column: "display_name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user",
                column: "login");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rubric_name",
                table: "tbl_rubric",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_name",
                table: "tbl_project",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_project_slug",
                table: "tbl_project",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_learning_goal_name",
                table: "tbl_learning_goal",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_learning_goal_slug",
                table: "tbl_learning_goal",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_name",
                table: "tbl_cursus",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_slug",
                table: "tbl_cursus",
                column: "slug");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review",
                column: "rubric_id",
                principalTable: "tbl_rubric",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_user_reviewer_id",
                table: "tbl_review",
                column: "reviewer_id",
                principalTable: "tbl_user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_user_reviewer_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_display_name",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_rubric_name",
                table: "tbl_rubric");

            migrationBuilder.DropIndex(
                name: "IX_tbl_project_name",
                table: "tbl_project");

            migrationBuilder.DropIndex(
                name: "IX_tbl_project_slug",
                table: "tbl_project");

            migrationBuilder.DropIndex(
                name: "IX_tbl_learning_goal_name",
                table: "tbl_learning_goal");

            migrationBuilder.DropIndex(
                name: "IX_tbl_learning_goal_slug",
                table: "tbl_learning_goal");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cursus_name",
                table: "tbl_cursus");

            migrationBuilder.DropIndex(
                name: "IX_tbl_cursus_slug",
                table: "tbl_cursus");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review",
                column: "rubric_id",
                principalTable: "tbl_rubric",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_user_reviewer_id",
                table: "tbl_review",
                column: "reviewer_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
