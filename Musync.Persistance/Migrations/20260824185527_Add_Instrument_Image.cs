using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_Instrument_Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Instruments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "instruments/guitar.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "instruments/piano.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "instruments/drums.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "instruments/bass.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 5,
                column: "Image",
                value: "instruments/violin.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 6,
                column: "Image",
                value: "instruments/saxophone.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 7,
                column: "Image",
                value: "instruments/trumpet.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 8,
                column: "Image",
                value: "instruments/flute.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 9,
                column: "Image",
                value: "instruments/cello.svg");

            migrationBuilder.UpdateData(
                table: "Instruments",
                keyColumn: "Id",
                keyValue: 10,
                column: "Image",
                value: "instruments/synthesizer.svg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Instruments");
        }
    }
}
