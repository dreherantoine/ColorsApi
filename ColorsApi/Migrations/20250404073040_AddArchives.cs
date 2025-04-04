using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddArchives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Palettes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Colors",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Colors");
        }
    }
}
