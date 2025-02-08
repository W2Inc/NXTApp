using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGitModelAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "git_commit",
                table: "tbl_git");

            migrationBuilder.RenameColumn(
                name: "source_kind",
                table: "tbl_git",
                newName: "git_provider");

            migrationBuilder.AddColumn<int>(
                name: "git_owner",
                table: "tbl_git",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "git_name",
                table: "tbl_git",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "git_namespace",
                table: "tbl_git",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "git_owner",
                table: "tbl_git");

            migrationBuilder.DropColumn(
                name: "git_name",
                table: "tbl_git");

            migrationBuilder.DropColumn(
                name: "git_namespace",
                table: "tbl_git");

            migrationBuilder.RenameColumn(
                name: "git_provider",
                table: "tbl_git",
                newName: "source_kind");

            migrationBuilder.AddColumn<string>(
                name: "git_commit",
                table: "tbl_git",
                type: "text",
                nullable: true);
        }
    }
}
