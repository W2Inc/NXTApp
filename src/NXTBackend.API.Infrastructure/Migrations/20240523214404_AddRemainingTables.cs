using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_git",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    git_url = table.Column<string>(type: "text", nullable: false),
                    git_branch = table.Column<string>(type: "text", nullable: false),
                    git_commit = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_git", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_review", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rubric",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rubric", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_goal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_goal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_project", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_git");

            migrationBuilder.DropTable(
                name: "tbl_review");

            migrationBuilder.DropTable(
                name: "tbl_rubric");

            migrationBuilder.DropTable(
                name: "tbl_user_goal");

            migrationBuilder.DropTable(
                name: "tbl_user_project");
        }
    }
}
