using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ProgressLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_progreso_lecciones_lecciones_LessonId",
                table: "progreso_lecciones");

            migrationBuilder.DropIndex(
                name: "IX_progreso_lecciones_LessonId",
                table: "progreso_lecciones");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "progreso_lecciones");

            migrationBuilder.CreateIndex(
                name: "IX_progreso_lecciones_leccion_id",
                table: "progreso_lecciones",
                column: "leccion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_progreso_lecciones_lecciones_leccion_id",
                table: "progreso_lecciones",
                column: "leccion_id",
                principalTable: "lecciones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_progreso_lecciones_lecciones_leccion_id",
                table: "progreso_lecciones");

            migrationBuilder.DropIndex(
                name: "IX_progreso_lecciones_leccion_id",
                table: "progreso_lecciones");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "progreso_lecciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_progreso_lecciones_LessonId",
                table: "progreso_lecciones",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_progreso_lecciones_lecciones_LessonId",
                table: "progreso_lecciones",
                column: "LessonId",
                principalTable: "lecciones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
