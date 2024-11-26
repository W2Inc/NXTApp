using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Enums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .Annotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .Annotation("Npgsql:Enum:notification_kind", "default,new_cursus,new_project,new_goal,new_event")
                .Annotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .Annotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .Annotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:cursus_kind", "dynamic,fixed")
                .OldAnnotation("Npgsql:Enum:member_invite_state", "pending,accepted")
                .OldAnnotation("Npgsql:Enum:notification_kind", "default,new_cursus,new_project,new_goal,new_event")
                .OldAnnotation("Npgsql:Enum:review_kind", "self,peer,async,auto")
                .OldAnnotation("Npgsql:Enum:review_state", "pending,in_progress,finished")
                .OldAnnotation("Npgsql:Enum:task_state", "inactive,active,awaiting,completed");
        }
    }
}
