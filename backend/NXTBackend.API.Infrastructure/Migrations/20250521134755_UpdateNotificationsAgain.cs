using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .OldAnnotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .OldAnnotation("Npgsql:Enum:notification_state", "none,read")
                .OldAnnotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .OldAnnotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .OldAnnotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");

            migrationBuilder.AddColumn<int>(
                name: "notification_preference",
                table: "tbl_user_details",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "tbl_notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notification_preference",
                table: "tbl_user_details");

            migrationBuilder.DropColumn(
                name: "state",
                table: "tbl_notifications");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .Annotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .Annotation("Npgsql:Enum:notification_state", "none,read")
                .Annotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .Annotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .Annotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");
        }
    }
}
