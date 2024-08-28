using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FooBooLooGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSessionNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "SessionNumbers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "SessionNumbers");
        }
    }
}
