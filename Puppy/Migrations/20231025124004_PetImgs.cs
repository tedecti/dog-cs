using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Puppy.Migrations
{
    /// <inheritdoc />
    public partial class PetImgs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Imgs",
                table: "Pet",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imgs",
                table: "Pet");
        }
    }
}
