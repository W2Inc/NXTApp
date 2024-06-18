using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "details_id",
                table: "tbl_user",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user",
                column: "details_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_tbl_user_details_details_id",
                table: "tbl_user",
                column: "details_id",
                principalTable: "tbl_user_details",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_tbl_user_details_details_id",
                table: "tbl_user");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_details_id",
                table: "tbl_user");

            migrationBuilder.DropColumn(
                name: "details_id",
                table: "tbl_user");
        }
    }
}
