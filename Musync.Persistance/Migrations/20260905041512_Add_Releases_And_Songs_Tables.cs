using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_Releases_And_Songs_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Cover = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Releases_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReleaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TrackNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Releases_BandId",
                table: "Releases",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ReleaseId",
                table: "Songs",
                column: "ReleaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Releases");
        }
    }
}
