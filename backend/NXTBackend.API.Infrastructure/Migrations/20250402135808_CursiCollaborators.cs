using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CursiCollaborators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollaboratosToCursi",
                columns: table => new
                {
                    CollaboratesOnCursiId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratosToCursi", x => new { x.CollaboratesOnCursiId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_CollaboratosToCursi_tbl_cursus_CollaboratesOnCursiId",
                        column: x => x.CollaboratesOnCursiId,
                        principalTable: "tbl_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratosToCursi_tbl_user_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratosToCursi_CollaboratorsId",
                table: "CollaboratosToCursi",
                column: "CollaboratorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratosToCursi");
        }
    }
}
