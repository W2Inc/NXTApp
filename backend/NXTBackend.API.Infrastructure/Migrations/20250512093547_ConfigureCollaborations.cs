using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCollaborations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratosToCursi");

            migrationBuilder.AddColumn<bool>(
                name: "deprecated",
                table: "tbl_project",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "deprecated",
                table: "tbl_learning_goal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "enabled",
                table: "tbl_learning_goal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "public",
                table: "tbl_learning_goal",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "deprecated",
                table: "tbl_cursus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CollaboratorsOnCursi",
                columns: table => new
                {
                    CollaboratedCursiId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorsOnCursi", x => new { x.CollaboratedCursiId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnCursi_tbl_cursus_CollaboratedCursiId",
                        column: x => x.CollaboratedCursiId,
                        principalTable: "tbl_cursus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnCursi_tbl_user_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollaboratorsOnGoals",
                columns: table => new
                {
                    CollaboratedGoalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollaboratorsOnGoals", x => new { x.CollaboratedGoalsId, x.CollaboratorsId });
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnGoals_tbl_learning_goal_CollaboratedGoalsId",
                        column: x => x.CollaboratedGoalsId,
                        principalTable: "tbl_learning_goal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollaboratorsOnGoals_tbl_user_CollaboratorsId",
                        column: x => x.CollaboratorsId,
                        principalTable: "tbl_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorsOnCursi_CollaboratorsId",
                table: "CollaboratorsOnCursi",
                column: "CollaboratorsId");

            migrationBuilder.CreateIndex(
                name: "IX_CollaboratorsOnGoals_CollaboratorsId",
                table: "CollaboratorsOnGoals",
                column: "CollaboratorsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollaboratorsOnCursi");

            migrationBuilder.DropTable(
                name: "CollaboratorsOnGoals");

            migrationBuilder.DropColumn(
                name: "deprecated",
                table: "tbl_project");

            migrationBuilder.DropColumn(
                name: "deprecated",
                table: "tbl_learning_goal");

            migrationBuilder.DropColumn(
                name: "enabled",
                table: "tbl_learning_goal");

            migrationBuilder.DropColumn(
                name: "public",
                table: "tbl_learning_goal");

            migrationBuilder.DropColumn(
                name: "deprecated",
                table: "tbl_cursus");

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
    }
}
