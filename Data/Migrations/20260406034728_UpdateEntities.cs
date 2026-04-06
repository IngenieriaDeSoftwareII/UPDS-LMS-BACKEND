using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
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

            migrationBuilder.CreateTable(
                name: "tareas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    leccion_id = table.Column<int>(type: "int", nullable: true),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fecha_apertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_entrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fecha_limite = table.Column<DateTime>(type: "datetime2", nullable: true),
                    url_archivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    formato = table.Column<int>(type: "int", nullable: true),
                    tamano_kb = table.Column<int>(type: "int", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    docente_id = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "activo"),
                    permite_entrega_tardia = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tareas", x => x.id);
                    table.ForeignKey(
                        name: "FK_tareas_lecciones_leccion_id",
                        column: x => x.leccion_id,
                        principalTable: "lecciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "entregas_tareas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tarea_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    url_archivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    formato = table.Column<int>(type: "int", nullable: true),
                    tamano_kb = table.Column<int>(type: "int", nullable: true),
                    comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revisado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    fecha_entrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "pendiente"),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entregas_tareas", x => x.id);
                    table.ForeignKey(
                        name: "FK_entregas_tareas_People_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entregas_tareas_tareas_tarea_id",
                        column: x => x.tarea_id,
                        principalTable: "tareas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_progreso_lecciones_leccion_id",
                table: "progreso_lecciones",
                column: "leccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_entregas_tareas_tarea_id",
                table: "entregas_tareas",
                column: "tarea_id");

            migrationBuilder.CreateIndex(
                name: "IX_entregas_tareas_usuario_id",
                table: "entregas_tareas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_tareas_leccion_id",
                table: "tareas",
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

            migrationBuilder.DropTable(
                name: "entregas_tareas");

            migrationBuilder.DropTable(
                name: "tareas");

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
