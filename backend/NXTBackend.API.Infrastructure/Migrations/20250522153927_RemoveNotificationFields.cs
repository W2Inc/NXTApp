using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NXTBackend.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "notifiable_id",
                table: "tbl_notifications");

            migrationBuilder.DropColumn(
                name: "resource_id",
                table: "tbl_notifications");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "tbl_notifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "tbl_notifications");

            migrationBuilder.AddColumn<string>(
                name: "data",
                table: "tbl_notifications",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "notifiable_id",
                table: "tbl_notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "resource_id",
                table: "tbl_notifications",
                type: "uuid",
                nullable: true);
        }
    }
}
