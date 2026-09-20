using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musync.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Add_Post_Caption_Length_And_Unique_Like_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The index below is unique, so it cannot be created over pre-existing duplicate
            // rows. Duplicates are possible on any database written to before this migration,
            // because LikePostCommandHandler check-then-inserts without a DB-level guard.
            // Keep the oldest like per (UserId, PostId) and drop the rest.
            migrationBuilder.Sql(
                "DELETE FROM PostLikes WHERE Id NOT IN (SELECT MIN(Id) FROM PostLikes GROUP BY UserId, PostId);");

            migrationBuilder.DropIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes",
                columns: new[] { "UserId", "PostId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_UserId_PostId",
                table: "PostLikes",
                columns: new[] { "UserId", "PostId" });
        }
    }
}
