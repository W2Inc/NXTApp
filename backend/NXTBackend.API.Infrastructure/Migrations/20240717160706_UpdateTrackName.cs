using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrackName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Track",
                table: "tbl_cursus",
                newName: "track");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "track",
                table: "tbl_cursus",
                newName: "Track");
        }
    }
}
