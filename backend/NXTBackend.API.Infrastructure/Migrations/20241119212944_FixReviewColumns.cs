using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixReviewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "tbl_review",
                newName: "kind");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "tbl_review",
                newName: "state");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "state",
                table: "tbl_review",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "kind",
                table: "tbl_review",
                newName: "name");
        }
    }
}
