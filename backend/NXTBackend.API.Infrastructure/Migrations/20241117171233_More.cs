using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class More : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "tbl_user_details",
                type: "character varying(16384)",
                maxLength: 16384,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "tbl_user_details",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16384)",
                oldMaxLength: 16384,
                oldNullable: true);
        }
    }
}
