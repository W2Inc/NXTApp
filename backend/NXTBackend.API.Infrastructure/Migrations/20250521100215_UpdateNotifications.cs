using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_user_notifications");

            migrationBuilder.DropIndex(
                name: "IX_tbl_notifications_kind",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "markdown",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "kind",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "message",
                table: "tbl_notifications");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .Annotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .Annotation("Npgsql:Enum:notification_state", "none,read")
                .Annotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .Annotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .Annotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed")
                .OldAnnotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .OldAnnotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .OldAnnotation("Npgsql:Enum:notification_kind", "default,invite,system")
                .OldAnnotation("Npgsql:Enum:notification_state", "none,read")
                .OldAnnotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .OldAnnotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .OldAnnotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");

            migrationBuilder.AddColumn<int>(
                name: "kind",
                table: "tbl_user_project_member",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "tbl_notifications",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "notifiable_id",
                table: "tbl_notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "read_at",
                table: "tbl_notifications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "git_hash",
                table: "tbl_git",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "kind",
                table: "tbl_user_project_member");

            migrationBuilder.DropColumn(
                name: "data",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "notifiable_id",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "read_at",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "git_hash",
                table: "tbl_git");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .Annotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .Annotation("Npgsql:Enum:notification_kind", "default,invite,system")
                .Annotation("Npgsql:Enum:notification_state", "none,read")
                .Annotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .Annotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .Annotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed")
                .OldAnnotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .OldAnnotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .OldAnnotation("Npgsql:Enum:notification_state", "none,read")
                .OldAnnotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .OldAnnotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .OldAnnotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");

            migrationBuilder.AddColumn<string>(
                name: "markdown",
                table: "tbl_project",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "kind",
                table: "tbl_notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "tbl_notifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "tbl_user_notifications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_user_notifications_tbl_notifications_notification_id",
                        column: x => x.notification_id,
                        principalTable: "tbl_notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_user_notifications_tbl_user_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_notifications_kind",
                table: "tbl_notifications",
                column: "kind");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_created_at",
                table: "tbl_user_notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_notification_id",
                table: "tbl_user_notifications",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_status",
                table: "tbl_user_notifications",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_user_id",
                table: "tbl_user_notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_notifications_user_id_notification_id",
                table: "tbl_user_notifications",
                columns: new[] { "user_id", "notification_id" },
                unique: true);
        }
    }
}
