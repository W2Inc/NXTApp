using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_comment_tbl_user_user_id",
                table: "tbl_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_project_tbl_user_creator_id",
                table: "tbl_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_rubric_tbl_user_creator_id",
                table: "tbl_rubric");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_feed_tbl_user_user_id",
                table: "tbl_user_feed");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_user_user_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_git_git_info_id",
                table: "tbl_user_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_project_project_id",
                table: "tbl_user_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_user_id",
                table: "tbl_user_project_member");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_feed_user_id",
                table: "tbl_user_feed");

            migrationBuilder.DropIndex(
                name: "IX_user_details_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_login_display",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_temporal_covering",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_notifications_notifiable_read",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_resource_created",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_state",
                table: "tbl_notifications");

            migrationBuilder.RenameIndex(
                name: "IX_user_login_unique",
                table: "tbl_user",
                newName: "IX_tbl_user_login");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user",
                column: "details_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login_display_name",
                table: "tbl_user",
                columns: new[] { "login", "display_name" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_user_feed_id",
                table: "tbl_user",
                column: "user_feed_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notifications_notifiable_id_read_at",
                table: "tbl_notifications",
                columns: new[] { "notifiable_id", "read_at" });

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment",
                column: "feedback_id",
                principalTable: "tbl_feedback",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_user_user_id",
                table: "tbl_comment",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_project_tbl_user_creator_id",
                table: "tbl_project",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_rubric_tbl_user_creator_id",
                table: "tbl_rubric",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_tbl_user_feed_user_feed_id",
                table: "tbl_user",
                column: "user_feed_id",
                principalTable: "tbl_user_feed",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                table: "tbl_user_goal",
                column: "goal_id",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                table: "tbl_user_goal",
                column: "user_cursus_id",
                principalTable: "tbl_user_cursus",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_user_user_id",
                table: "tbl_user_goal",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_git_git_info_id",
                table: "tbl_user_project",
                column: "git_info_id",
                principalTable: "tbl_git",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_project_project_id",
                table: "tbl_user_project",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_user_id",
                table: "tbl_user_project_member",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_comment_tbl_user_user_id",
                table: "tbl_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_project_tbl_user_creator_id",
                table: "tbl_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_rubric_tbl_user_creator_id",
                table: "tbl_rubric");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_tbl_user_feed_user_feed_id",
                table: "tbl_user");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_user_user_id",
                table: "tbl_user_goal");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_git_git_info_id",
                table: "tbl_user_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_project_project_id",
                table: "tbl_user_project");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_user_id",
                table: "tbl_user_project_member");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_login_display_name",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_user_feed_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_notifications_notifiable_id_read_at",
                table: "tbl_notifications");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user",
                newName: "IX_user_login_unique");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_feed_user_id",
                table: "tbl_user_feed",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_details_id",
                table: "tbl_user",
                column: "details_id",
                filter: "details_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_login_display",
                table: "tbl_user",
                columns: new[] { "login", "display_name" })
                .Annotation("Npgsql:IndexInclude", new[] { "avatar_url", "created_at", "updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_user_temporal_covering",
                table: "tbl_user",
                column: "created_at",
                descending: new bool[0])
                .Annotation("Npgsql:IndexInclude", new[] { "login", "display_name", "avatar_url", "updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_notifiable_read",
                table: "tbl_notifications",
                columns: new[] { "notifiable_id", "read_at" })
                .Annotation("Npgsql:IndexInclude", new[] { "created_at", "descriptor", "type" });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_resource_created",
                table: "tbl_notifications",
                columns: new[] { "resource_id", "created_at" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_notifications_state",
                table: "tbl_notifications",
                column: "state",
                filter: "state = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment",
                column: "feedback_id",
                principalTable: "tbl_feedback",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_user_user_id",
                table: "tbl_comment",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_project_tbl_user_creator_id",
                table: "tbl_project",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_rubric_tbl_user_creator_id",
                table: "tbl_rubric",
                column: "creator_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_feed_tbl_user_user_id",
                table: "tbl_user_feed",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_learning_goal_goal_id",
                table: "tbl_user_goal",
                column: "goal_id",
                principalTable: "tbl_learning_goal",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_user_cursus_user_cursus_id",
                table: "tbl_user_goal",
                column: "user_cursus_id",
                principalTable: "tbl_user_cursus",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_user_user_id",
                table: "tbl_user_goal",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_git_git_info_id",
                table: "tbl_user_project",
                column: "git_info_id",
                principalTable: "tbl_git",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_project_project_id",
                table: "tbl_user_project",
                column: "project_id",
                principalTable: "tbl_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_user_id",
                table: "tbl_user_project_member",
                column: "user_id",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
