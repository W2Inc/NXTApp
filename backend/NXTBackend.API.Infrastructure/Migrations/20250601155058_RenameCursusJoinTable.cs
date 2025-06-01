using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCursusJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_cursus_CursusId",
                table: "rel_CursusCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_user_UserId",
                table: "rel_CursusCollaborator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rel_CursusCollaborator",
                table: "rel_CursusCollaborator");

            migrationBuilder.RenameTable(
                name: "rel_CursusCollaborator",
                newName: "rel_cursuscollaborator");

            migrationBuilder.RenameIndex(
                name: "IX_rel_CursusCollaborator_CursusId",
                table: "rel_cursuscollaborator",
                newName: "IX_rel_cursuscollaborator_CursusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rel_cursuscollaborator",
                table: "rel_cursuscollaborator",
                columns: new[] { "UserId", "CursusId" });

            migrationBuilder.AddForeignKey(
                name: "FK_rel_cursuscollaborator_tbl_cursus_CursusId",
                table: "rel_cursuscollaborator",
                column: "CursusId",
                principalTable: "tbl_cursus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_cursuscollaborator_tbl_user_UserId",
                table: "rel_cursuscollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rel_cursuscollaborator_tbl_cursus_CursusId",
                table: "rel_cursuscollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_rel_cursuscollaborator_tbl_user_UserId",
                table: "rel_cursuscollaborator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rel_cursuscollaborator",
                table: "rel_cursuscollaborator");

            migrationBuilder.RenameTable(
                name: "rel_cursuscollaborator",
                newName: "rel_CursusCollaborator");

            migrationBuilder.RenameIndex(
                name: "IX_rel_cursuscollaborator_CursusId",
                table: "rel_CursusCollaborator",
                newName: "IX_rel_CursusCollaborator_CursusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rel_CursusCollaborator",
                table: "rel_CursusCollaborator",
                columns: new[] { "UserId", "CursusId" });

            migrationBuilder.AddForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_cursus_CursusId",
                table: "rel_CursusCollaborator",
                column: "CursusId",
                principalTable: "tbl_cursus",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rel_CursusCollaborator_tbl_user_UserId",
                table: "rel_CursusCollaborator",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
