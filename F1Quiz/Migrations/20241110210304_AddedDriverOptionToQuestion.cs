using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Quiz.Migrations
{
    /// <inheritdoc />
    public partial class AddedDriverOptionToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverOptionsJson",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverOptionsJson",
                table: "Questions");
        }
    }
}
