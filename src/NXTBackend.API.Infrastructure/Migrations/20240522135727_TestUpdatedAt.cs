using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "details_id",
                table: "tbl_user");

            migrationBuilder.RenameColumn(
                name: "details_id",
                table: "tbl_user_details",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "tbl_user",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "feature_id",
                table: "tbl_feature",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "tbl_user_details",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_user_details",
                newName: "details_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_user",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tbl_feature",
                newName: "feature_id");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "tbl_user_details",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AddColumn<Guid>(
                name: "details_id",
                table: "tbl_user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
