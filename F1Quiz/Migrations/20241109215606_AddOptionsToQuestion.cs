﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F1Quiz.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionsToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Options",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                table: "Questions");
        }
    }
}
