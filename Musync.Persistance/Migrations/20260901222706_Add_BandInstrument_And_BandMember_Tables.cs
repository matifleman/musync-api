using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_BandInstrument_And_BandMember_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BandInstrument",
                columns: table => new
                {
                    BandId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstrumentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandInstrument", x => new { x.BandId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_BandInstrument_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandInstrument_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BandId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    CreatedById = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    UpdatedById = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandMembers_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BandMembers_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandInstrument_InstrumentId",
                table: "BandInstrument",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_BandId_InstrumentId",
                table: "BandMembers",
                columns: new[] { "BandId", "InstrumentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_BandId_UserId",
                table: "BandMembers",
                columns: new[] { "BandId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_InstrumentId",
                table: "BandMembers",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BandMembers_UserId",
                table: "BandMembers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BandInstrument");

            migrationBuilder.DropTable(
                name: "BandMembers");
        }
    }
}
