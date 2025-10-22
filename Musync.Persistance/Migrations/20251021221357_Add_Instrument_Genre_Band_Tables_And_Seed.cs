using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_Instrument_Genre_Band_Tables_And_Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserGenre",
                columns: table => new
                {
                    FavoriteGenresId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserGenre", x => new { x.FavoriteGenresId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserGenre_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserGenre_Genres_FavoriteGenresId",
                        column: x => x.FavoriteGenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandGenre",
                columns: table => new
                {
                    BandsId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenresId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandGenre", x => new { x.BandsId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_BandGenre_Bands_BandsId",
                        column: x => x.BandsId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserInstrument",
                columns: table => new
                {
                    FavoriteInstrumentsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserInstrument", x => new { x.FavoriteInstrumentsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserInstrument_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserInstrument_Instruments_FavoriteInstrumentsId",
                        column: x => x.FavoriteInstrumentsId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, null, null, "Rock", null, null },
                    { 2, null, null, "Pop", null, null },
                    { 3, null, null, "Jazz", null, null },
                    { 4, null, null, "Classical", null, null },
                    { 5, null, null, "Hip Hop", null, null },
                    { 6, null, null, "Reggae", null, null },
                    { 7, null, null, "Electronic", null, null },
                    { 8, null, null, "Folk", null, null },
                    { 9, null, null, "Metal", null, null },
                    { 10, null, null, "Blues", null, null }
                });

            migrationBuilder.InsertData(
                table: "Instruments",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, null, null, "Guitar", null, null },
                    { 2, null, null, "Piano", null, null },
                    { 3, null, null, "Drums", null, null },
                    { 4, null, null, "Bass", null, null },
                    { 5, null, null, "Violin", null, null },
                    { 6, null, null, "Saxophone", null, null },
                    { 7, null, null, "Trumpet", null, null },
                    { 8, null, null, "Flute", null, null },
                    { 9, null, null, "Cello", null, null },
                    { 10, null, null, "Synthesizer", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserGenre_UsersId",
                table: "ApplicationUserGenre",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserInstrument_UsersId",
                table: "ApplicationUserInstrument",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_BandGenre_GenresId",
                table: "BandGenre",
                column: "GenresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserGenre");

            migrationBuilder.DropTable(
                name: "ApplicationUserInstrument");

            migrationBuilder.DropTable(
                name: "BandGenre");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Bands");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");
        }
    }
}
