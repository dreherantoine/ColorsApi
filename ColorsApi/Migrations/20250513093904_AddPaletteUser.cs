using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPaletteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Palettes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Palettes",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.NewGuid());

            migrationBuilder.CreateIndex(
                name: "IX_Palettes_UserId",
                table: "Palettes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Palettes_Users_UserId",
                table: "Palettes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Palettes_Users_UserId",
                table: "Palettes");

            migrationBuilder.DropIndex(
                name: "IX_Palettes_UserId",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Palettes");
        }
    }
}
