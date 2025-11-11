using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Posts_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Posts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Posts");
        }
    }
}
