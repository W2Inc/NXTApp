using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnonymousReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "anonymous",
                table: "tbl_review",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "anonymous",
                table: "tbl_review");
        }
    }
}
