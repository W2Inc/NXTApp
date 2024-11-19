using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RubricsDecidedByReviewerAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_rubric_rubric_id",
                table: "tbl_user_project");

            migrationBuilder.RenameColumn(
                name: "rubric_id",
                table: "tbl_user_project",
                newName: "RubricId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_user_project_rubric_id",
                table: "tbl_user_project",
                newName: "IX_tbl_user_project_RubricId");

            migrationBuilder.AlterColumn<Guid>(
                name: "RubricId",
                table: "tbl_user_project",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_rubric_RubricId",
                table: "tbl_user_project",
                column: "RubricId",
                principalTable: "tbl_rubric",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_project_tbl_rubric_RubricId",
                table: "tbl_user_project");

            migrationBuilder.RenameColumn(
                name: "RubricId",
                table: "tbl_user_project",
                newName: "rubric_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_user_project_RubricId",
                table: "tbl_user_project",
                newName: "IX_tbl_user_project_rubric_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "rubric_id",
                table: "tbl_user_project",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_project_tbl_rubric_rubric_id",
                table: "tbl_user_project",
                column: "rubric_id",
                principalTable: "tbl_rubric",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
