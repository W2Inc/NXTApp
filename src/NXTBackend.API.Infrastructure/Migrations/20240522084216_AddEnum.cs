using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "feature_ID",
                table: "tbl_feature",
                newName: "feature_id");

            migrationBuilder.CreateTable(
                name: "tbl_user_details",
                columns: table => new
                {
                    details_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    github_url = table.Column<string>(type: "text", nullable: false),
                    linkedin_url = table.Column<string>(type: "text", nullable: false),
                    twitter_url = table.Column<string>(type: "text", nullable: false),
                    website_url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_details", x => x.details_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_user_details");

            migrationBuilder.RenameColumn(
                name: "feature_id",
                table: "tbl_feature",
                newName: "feature_ID");
        }
    }
}
