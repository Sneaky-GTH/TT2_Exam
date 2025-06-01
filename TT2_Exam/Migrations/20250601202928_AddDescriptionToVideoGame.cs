using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TT2_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToVideoGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VideoGames",
                type: "varchar(2047)",
                maxLength: 2047,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "VideoGames");
        }
    }
}
