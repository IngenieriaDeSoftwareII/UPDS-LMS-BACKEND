using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixImageVideoRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageContentId",
                table: "Contents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VideoContentId",
                table: "Contents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contents_ImageContentId",
                table: "Contents",
                column: "ImageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_VideoContentId",
                table: "Contents",
                column: "VideoContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_ImageContents_ImageContentId",
                table: "Contents",
                column: "ImageContentId",
                principalTable: "ImageContents",
                principalColumn: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_VideoContents_VideoContentId",
                table: "Contents",
                column: "VideoContentId",
                principalTable: "VideoContents",
                principalColumn: "ContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_ImageContents_ImageContentId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_VideoContents_VideoContentId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_ImageContentId",
                table: "Contents");

            migrationBuilder.DropIndex(
                name: "IX_Contents_VideoContentId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ImageContentId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "VideoContentId",
                table: "Contents");
        }
    }
}
