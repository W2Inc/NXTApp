using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserAndNotificationIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_display_name",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_notifications_created_at",
                table: "tbl_notifications");

            migrationBuilder.CreateIndex(
                name: "IX_user_activity",
                table: "tbl_user",
                columns: new[] { "updated_at", "created_at" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_user_details_id",
                table: "tbl_user",
                column: "details_id",
                filter: "details_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_display_name",
                table: "tbl_user",
                column: "display_name",
                filter: "display_name IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_feed_id",
                table: "tbl_user",
                column: "user_feed_id",
                unique: true,
                filter: "user_feed_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_user_list_covering",
                table: "tbl_user",
                column: "created_at",
                descending: new bool[0])
                .Annotation("Npgsql:IndexInclude", new[] { "login", "display_name", "avatar_url", "updated_at" });

            migrationBuilder.CreateIndex(
                name: "IX_user_login_display",
                table: "tbl_user",
                columns: new[] { "login", "display_name" })
                .Annotation("Npgsql:IndexInclude", new[] { "avatar_url", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_user_login_unique",
                table: "tbl_user",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_updated_at",
                table: "tbl_user",
                column: "updated_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_created_at",
                table: "tbl_notifications",
                column: "created_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_data_gin",
                table: "tbl_notifications",
                column: "data")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_notifiable_descriptor",
                table: "tbl_notifications",
                columns: new[] { "notifiable_id", "descriptor" });

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

            migrationBuilder.CreateIndex(
                name: "IX_notifications_state_created",
                table: "tbl_notifications",
                columns: new[] { "state", "created_at" },
                filter: "read_at IS NOT NULL OR state != 0");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_type",
                table: "tbl_notifications",
                column: "type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_activity",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_details_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_display_name",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_feed_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_list_covering",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_login_display",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_login_unique",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_user_updated_at",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_notifications_created_at",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_data_gin",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_notifiable_descriptor",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_notifiable_read",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_resource_created",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_state",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_state_created",
                table: "tbl_notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_type",
                table: "tbl_notifications");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user",
                column: "details_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_display_name",
                table: "tbl_user",
                column: "display_name");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login",
                table: "tbl_user",
                column: "login");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notifications_created_at",
                table: "tbl_notifications",
                column: "created_at");
        }
    }
}
