using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoVertices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_goal_tbl_cursus_vertex_vertex_id",
                table: "tbl_user_goal");

            migrationBuilder.DropTable(
                name: "CursusVertexLearningGoal");

            migrationBuilder.DropTable(
                name: "tbl_cursus_vertex");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_goal_vertex_id",
                table: "tbl_user_goal");

            migrationBuilder.DropColumn(
                name: "vertex_id",
                table: "tbl_user_goal");

            migrationBuilder.AddColumn<byte[]>(
                name: "Track",
                table: "tbl_cursus",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Track",
                table: "tbl_cursus");

            migrationBuilder.AddColumn<Guid>(
                name: "vertex_id",
                table: "tbl_user_goal",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "tbl_cursus_vertex",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cursus_id = table.Column<Guid>(type: "uuid", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_cursus_vertex", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_cursus_vertex_tbl_cursus_cursus_id",
                        column: x => x.cursus_id,
                        principalTable: "tbl_cursus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tbl_cursus_vertex_tbl_cursus_vertex_parent_id",
                        column: x => x.parent_id,
                        principalTable: "tbl_cursus_vertex",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CursusVertexLearningGoal",
                columns: table => new
                {
                    GoalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    VerticesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CursusVertexLearningGoal", x => new { x.GoalsId, x.VerticesId });
                    table.ForeignKey(
                        name: "FK_CursusVertexLearningGoal_tbl_cursus_vertex_VerticesId",
                        column: x => x.VerticesId,
                        principalTable: "tbl_cursus_vertex",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CursusVertexLearningGoal_tbl_learning_goal_GoalsId",
                        column: x => x.GoalsId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_goal_vertex_id",
                table: "tbl_user_goal",
                column: "vertex_id");

            migrationBuilder.CreateIndex(
                name: "IX_CursusVertexLearningGoal_VerticesId",
                table: "CursusVertexLearningGoal",
                column: "VerticesId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_vertex_cursus_id",
                table: "tbl_cursus_vertex",
                column: "cursus_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_cursus_vertex_parent_id",
                table: "tbl_cursus_vertex",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_goal_tbl_cursus_vertex_vertex_id",
                table: "tbl_user_goal",
                column: "vertex_id",
                principalTable: "tbl_cursus_vertex",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
