using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_feed_tbl_user_actor_id",
                table: "tbl_feed");

            migrationBuilder.DropIndex(
                name: "IX_tbl_feed_actor_id",
                table: "tbl_feed");

            migrationBuilder.RenameColumn(
                name: "actor_id",
                table: "tbl_feed",
                newName: "notifiable_id");

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "tbl_notifications",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "tbl_feed",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_feed_UserId",
                table: "tbl_feed",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_feed_tbl_user_UserId",
                table: "tbl_feed",
                column: "UserId",
                principalTable: "tbl_user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_feed_tbl_user_UserId",
                table: "tbl_feed");

            migrationBuilder.DropIndex(
                name: "IX_tbl_feed_UserId",
                table: "tbl_feed");

            migrationBuilder.DropColumn(
                name: "data",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tbl_feed");

            migrationBuilder.RenameColumn(
                name: "notifiable_id",
                table: "tbl_feed",
                newName: "actor_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_feed_actor_id",
                table: "tbl_feed",
                column: "actor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_feed_tbl_user_actor_id",
                table: "tbl_feed",
                column: "actor_id",
                principalTable: "tbl_user",
                principalColumn: "id");
        }
    }
}
