using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_member_tbl_user_project_user_project_id",
                table: "tbl_user_project_member",
                column: "user_project_id",
                principalTable: "tbl_user_project",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
