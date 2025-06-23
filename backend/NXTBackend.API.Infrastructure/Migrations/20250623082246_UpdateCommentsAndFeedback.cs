using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommentsAndFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_feedback_feedback_id",
                table: "tbl_review");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review");

            migrationBuilder.DropIndex(
                name: "IX_tbl_review_feedback_id",
                table: "tbl_review");

            migrationBuilder.DropColumn(
                name: "anonymous",
                table: "tbl_review");

            migrationBuilder.DropColumn(
                name: "feedback_id",
                table: "tbl_review");

            migrationBuilder.DropColumn(
                name: "validated",
                table: "tbl_review");

            migrationBuilder.RenameColumn(
                name: "rubric_id",
                table: "tbl_review",
                newName: "RubricId");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_review_rubric_id",
                table: "tbl_review",
                newName: "IX_tbl_review_RubricId");

            migrationBuilder.RenameColumn(
                name: "feedback_id",
                table: "tbl_comment",
                newName: "parent_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_comment_feedback_id",
                table: "tbl_comment",
                newName: "IX_tbl_comment_parent_id");

            migrationBuilder.AddColumn<string>(
                name: "hash",
                table: "tbl_review",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "kind",
                table: "tbl_feedback",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "parent_type",
                table: "tbl_comment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_rubric_RubricId",
                table: "tbl_review",
                column: "RubricId",
                principalTable: "tbl_rubric",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_review_tbl_rubric_RubricId",
                table: "tbl_review");

            migrationBuilder.DropColumn(
                name: "hash",
                table: "tbl_review");

            migrationBuilder.DropColumn(
                name: "kind",
                table: "tbl_feedback");

            migrationBuilder.DropColumn(
                name: "parent_type",
                table: "tbl_comment");

            migrationBuilder.RenameColumn(
                name: "RubricId",
                table: "tbl_review",
                newName: "rubric_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_review_RubricId",
                table: "tbl_review",
                newName: "IX_tbl_review_rubric_id");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "tbl_comment",
                newName: "feedback_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_comment_parent_id",
                table: "tbl_comment",
                newName: "IX_tbl_comment_feedback_id");

            migrationBuilder.AddColumn<bool>(
                name: "anonymous",
                table: "tbl_review",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "feedback_id",
                table: "tbl_review",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "validated",
                table: "tbl_review",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_review_feedback_id",
                table: "tbl_review",
                column: "feedback_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_comment_tbl_feedback_feedback_id",
                table: "tbl_comment",
                column: "feedback_id",
                principalTable: "tbl_feedback",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_feedback_feedback_id",
                table: "tbl_review",
                column: "feedback_id",
                principalTable: "tbl_feedback",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_review_tbl_rubric_rubric_id",
                table: "tbl_review",
                column: "rubric_id",
                principalTable: "tbl_rubric",
                principalColumn: "id");
        }
    }
}
