using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Quiz.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToGuidResponseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new temporary column
            migrationBuilder.AddColumn<Guid>(
                name: "UserIdNew",
                table: "Responses",  // Replace with your actual table name
                nullable: true);

            // Generate GUIDs for existing rows
            migrationBuilder.Sql(@"
            UPDATE Responses 
            SET UserIdNew = NEWID()
            WHERE UserIdNew IS NULL");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Responses");  // Replace with your actual table name

            // Rename the new column
            migrationBuilder.RenameColumn(
                name: "UserIdNew",
                table: "Responses",   // Replace with your actual table name
                newName: "UserId");

            // Make it non-nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Responses",   // Replace with your actual table name
                nullable: false,
                defaultValue: new Guid());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Responses",   // Replace with your actual table name
                nullable: false,
                defaultValue: 0);
        }
    }
}
