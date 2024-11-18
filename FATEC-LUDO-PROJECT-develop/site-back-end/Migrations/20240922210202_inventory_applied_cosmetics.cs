using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudoAPI.Migrations
{
    /// <inheritdoc />
    public partial class inventory_applied_cosmetics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "applied_cosmetics",
                table: "user_cosmetics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "applied_cosmetics",
                table: "user_cosmetics");
        }
    }
}
