using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPaletteToColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Palettes_PaletteId1",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Colors_PaletteId1",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "PaletteId1",
                table: "Colors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaletteId1",
                table: "Colors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_PaletteId1",
                table: "Colors",
                column: "PaletteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Palettes_PaletteId1",
                table: "Colors",
                column: "PaletteId1",
                principalTable: "Palettes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
