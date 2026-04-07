using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_People_PersonId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "catalogos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "docentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    especialidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    biografia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_cursos = table.Column<int>(type: "int", nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_docentes", x => x.id);
                    table.ForeignKey(
                        name: "FK_docentes_AspNetUsers_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false),
                    catalogo_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id);
                    table.ForeignKey(
                        name: "FK_categorias_catalogos_catalogo_id",
                        column: x => x.catalogo_id,
                        principalTable: "catalogos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "cursos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    docente_id = table.Column<int>(type: "int", nullable: true),
                    nivel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    categoria_id = table.Column<int>(type: "int", nullable: true),
                    imagen_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publicado = table.Column<bool>(type: "bit", nullable: false),
                    duracion_total_min = table.Column<int>(type: "int", nullable: false),
                    max_estudiantes = table.Column<int>(type: "int", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_cursos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cursos_docentes_docente_id",
                        column: x => x.docente_id,
                        principalTable: "docentes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "inscripciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    curso_id = table.Column<int>(type: "int", nullable: false),
                    fecha_completado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inscripciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_inscripciones_People_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_inscripciones_cursos_curso_id",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "modulos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    curso_id = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: true, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modulos", x => x.id);
                    table.ForeignKey(
                        name: "FK_modulos_cursos_curso_id",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lecciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modulo_id = table.Column<int>(type: "int", nullable: true),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orden = table.Column<int>(type: "int", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_lecciones_modulos_modulo_id",
                        column: x => x.modulo_id,
                        principalTable: "modulos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nota_final_modulo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estudiante_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    modulo_id = table.Column<int>(type: "int", nullable: false),
                    nota_ponderada = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    items_calificados = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    items_totales = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nota_final_modulo", x => x.id);
                    table.ForeignKey(
                        name: "FK_nota_final_modulo_AspNetUsers_estudiante_id",
                        column: x => x.estudiante_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_nota_final_modulo_modulos_modulo_id",
                        column: x => x.modulo_id,
                        principalTable: "modulos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "actividad_entregas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    leccion_id = table.Column<int>(type: "int", nullable: false),
                    url_archivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comentario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividad_entregas", x => x.id);
                    table.ForeignKey(
                        name: "FK_actividad_entregas_AspNetUsers_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_actividad_entregas_lecciones_leccion_id",
                        column: x => x.leccion_id,
                        principalTable: "lecciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "evaluaciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    curso_id = table.Column<int>(type: "int", nullable: false),
                    leccion_id = table.Column<int>(type: "int", nullable: true),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    puntaje_maximo = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    puntaje_minimo_aprobacion = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    intentos_permitidos = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    tiempo_limite_max = table.Column<int>(type: "int", nullable: true),
                    fecha_limite = table.Column<DateTime>(type: "datetime2", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluaciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_evaluaciones_cursos_curso_id",
                        column: x => x.curso_id,
                        principalTable: "cursos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluaciones_lecciones_leccion_id",
                        column: x => x.leccion_id,
                        principalTable: "lecciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "progreso_lecciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    leccion_id = table.Column<int>(type: "int", nullable: false),
                    completado = table.Column<bool>(type: "bit", nullable: true),
                    posicion_actual = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    fecha_completado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_progreso_lecciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_progreso_lecciones_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_progreso_lecciones_lecciones_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lecciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entregas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    evaluacion_id = table.Column<int>(type: "int", nullable: true),
                    estudiante_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    contenido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    archivo_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entregas", x => x.id);
                    table.ForeignKey(
                        name: "FK_entregas_AspNetUsers_estudiante_id",
                        column: x => x.estudiante_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_entregas_evaluaciones_evaluacion_id",
                        column: x => x.evaluacion_id,
                        principalTable: "evaluaciones",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "intentos_evaluacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    evaluacion_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    numero_intento = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    puntaje_obtenido = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    es_aprobado = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intentos_evaluacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_intentos_evaluacion_evaluaciones_evaluacion_id",
                        column: x => x.evaluacion_id,
                        principalTable: "evaluaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "items_calificables",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modulo_id = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    evaluacion_id = table.Column<int>(type: "int", nullable: true),
                    leccion_id = table.Column<int>(type: "int", nullable: true),
                    ponderacion = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items_calificables", x => x.id);
                    table.ForeignKey(
                        name: "FK_items_calificables_evaluaciones_evaluacion_id",
                        column: x => x.evaluacion_id,
                        principalTable: "evaluaciones",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_items_calificables_lecciones_leccion_id",
                        column: x => x.leccion_id,
                        principalTable: "lecciones",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_items_calificables_modulos_modulo_id",
                        column: x => x.modulo_id,
                        principalTable: "modulos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "preguntas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    evaluacion_id = table.Column<int>(type: "int", nullable: false),
                    enunciado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    puntos = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preguntas", x => x.id);
                    table.ForeignKey(
                        name: "FK_preguntas_evaluaciones_evaluacion_id",
                        column: x => x.evaluacion_id,
                        principalTable: "evaluaciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "calificaciones_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    estudiante_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    docente_id = table.Column<int>(type: "int", nullable: false),
                    nota = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entrega_id = table.Column<int>(type: "int", nullable: true),
                    actividad_entrega_id = table.Column<int>(type: "int", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calificaciones_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_calificaciones_items_AspNetUsers_estudiante_id",
                        column: x => x.estudiante_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificaciones_items_actividad_entregas_actividad_entrega_id",
                        column: x => x.actividad_entrega_id,
                        principalTable: "actividad_entregas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificaciones_items_docentes_docente_id",
                        column: x => x.docente_id,
                        principalTable: "docentes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificaciones_items_entregas_entrega_id",
                        column: x => x.entrega_id,
                        principalTable: "entregas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_calificaciones_items_items_calificables_item_id",
                        column: x => x.item_id,
                        principalTable: "items_calificables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opciones_respuesta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pregunta_id = table.Column<int>(type: "int", nullable: false),
                    texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    es_correcta = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    orden = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opciones_respuesta", x => x.id);
                    table.ForeignKey(
                        name: "FK_opciones_respuesta_preguntas_pregunta_id",
                        column: x => x.pregunta_id,
                        principalTable: "preguntas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "respuestas_evaluacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intento_id = table.Column<int>(type: "int", nullable: false),
                    pregunta_id = table.Column<int>(type: "int", nullable: false),
                    opcion_respuesta_id = table.Column<int>(type: "int", nullable: true),
                    respuesta_texto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entity_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_respuestas_evaluacion", x => x.id);
                    table.ForeignKey(
                        name: "FK_respuestas_evaluacion_intentos_evaluacion_intento_id",
                        column: x => x.intento_id,
                        principalTable: "intentos_evaluacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_respuestas_evaluacion_opciones_respuesta_opcion_respuesta_id",
                        column: x => x.opcion_respuesta_id,
                        principalTable: "opciones_respuesta",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_respuestas_evaluacion_preguntas_pregunta_id",
                        column: x => x.pregunta_id,
                        principalTable: "preguntas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contenidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    leccion_id = table.Column<int>(type: "int", nullable: false),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false),
                    duracion = table.Column<int>(type: "int", nullable: false),
                    entity_status = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VideoContenidoId = table.Column<int>(type: "int", nullable: true),
                    DocumentoContenidoId = table.Column<int>(type: "int", nullable: true),
                    ImagenContenidoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contenidos", x => x.id);
                    table.ForeignKey(
                        name: "FK_contenidos_lecciones_leccion_id",
                        column: x => x.leccion_id,
                        principalTable: "lecciones",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contenidos_documento",
                columns: table => new
                {
                    contenido_id = table.Column<int>(type: "int", nullable: false),
                    url_archivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    formato = table.Column<int>(type: "int", nullable: false),
                    tamano_kb = table.Column<int>(type: "int", nullable: true),
                    num_paginas = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contenidos_documento", x => x.contenido_id);
                    table.ForeignKey(
                        name: "FK_contenidos_documento_contenidos_contenido_id",
                        column: x => x.contenido_id,
                        principalTable: "contenidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contenidos_imagen",
                columns: table => new
                {
                    contenido_id = table.Column<int>(type: "int", nullable: false),
                    url_imagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    formato = table.Column<int>(type: "int", nullable: false),
                    ancho_px = table.Column<int>(type: "int", nullable: true),
                    alto_px = table.Column<int>(type: "int", nullable: true),
                    texto_alternativo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    tamano_kb = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contenidos_imagen", x => x.contenido_id);
                    table.ForeignKey(
                        name: "FK_contenidos_imagen_contenidos_contenido_id",
                        column: x => x.contenido_id,
                        principalTable: "contenidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contenidos_video",
                columns: table => new
                {
                    contenido_id = table.Column<int>(type: "int", nullable: false),
                    url_video = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    duracion_seg = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contenidos_video", x => x.contenido_id);
                    table.ForeignKey(
                        name: "FK_contenidos_video_contenidos_contenido_id",
                        column: x => x.contenido_id,
                        principalTable: "contenidos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_actividad_entregas_leccion_id",
                table: "actividad_entregas",
                column: "leccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_actividad_entregas_usuario_id",
                table: "actividad_entregas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_items_actividad_entrega_id",
                table: "calificaciones_items",
                column: "actividad_entrega_id");

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_items_docente_id",
                table: "calificaciones_items",
                column: "docente_id");

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_items_entrega_id",
                table: "calificaciones_items",
                column: "entrega_id");

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_items_estudiante_id",
                table: "calificaciones_items",
                column: "estudiante_id");

            migrationBuilder.CreateIndex(
                name: "IX_calificaciones_items_item_id",
                table: "calificaciones_items",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_categorias_catalogo_id",
                table: "categorias",
                column: "catalogo_id");

            migrationBuilder.CreateIndex(
                name: "IX_contenidos_DocumentoContenidoId",
                table: "contenidos",
                column: "DocumentoContenidoId");

            migrationBuilder.CreateIndex(
                name: "IX_contenidos_ImagenContenidoId",
                table: "contenidos",
                column: "ImagenContenidoId");

            migrationBuilder.CreateIndex(
                name: "IX_contenidos_leccion_id",
                table: "contenidos",
                column: "leccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_contenidos_VideoContenidoId",
                table: "contenidos",
                column: "VideoContenidoId");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_categoria_id",
                table: "cursos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_cursos_docente_id",
                table: "cursos",
                column: "docente_id");

            migrationBuilder.CreateIndex(
                name: "IX_docentes_usuario_id",
                table: "docentes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_entregas_estudiante_id",
                table: "entregas",
                column: "estudiante_id");

            migrationBuilder.CreateIndex(
                name: "IX_entregas_evaluacion_id",
                table: "entregas",
                column: "evaluacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluaciones_curso_id",
                table: "evaluaciones",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluaciones_leccion_id",
                table: "evaluaciones",
                column: "leccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_inscripciones_curso_id",
                table: "inscripciones",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_inscripciones_usuario_id",
                table: "inscripciones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_intentos_evaluacion_evaluacion_id",
                table: "intentos_evaluacion",
                column: "evaluacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_calificables_evaluacion_id",
                table: "items_calificables",
                column: "evaluacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_calificables_leccion_id",
                table: "items_calificables",
                column: "leccion_id");

            migrationBuilder.CreateIndex(
                name: "IX_items_calificables_modulo_id",
                table: "items_calificables",
                column: "modulo_id");

            migrationBuilder.CreateIndex(
                name: "IX_lecciones_modulo_id",
                table: "lecciones",
                column: "modulo_id");

            migrationBuilder.CreateIndex(
                name: "IX_modulos_curso_id",
                table: "modulos",
                column: "curso_id");

            migrationBuilder.CreateIndex(
                name: "IX_nota_final_modulo_estudiante_id",
                table: "nota_final_modulo",
                column: "estudiante_id");

            migrationBuilder.CreateIndex(
                name: "IX_nota_final_modulo_modulo_id",
                table: "nota_final_modulo",
                column: "modulo_id");

            migrationBuilder.CreateIndex(
                name: "IX_opciones_respuesta_pregunta_id",
                table: "opciones_respuesta",
                column: "pregunta_id");

            migrationBuilder.CreateIndex(
                name: "IX_preguntas_evaluacion_id",
                table: "preguntas",
                column: "evaluacion_id");

            migrationBuilder.CreateIndex(
                name: "IX_progreso_lecciones_LessonId",
                table: "progreso_lecciones",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_progreso_lecciones_UserId",
                table: "progreso_lecciones",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_evaluacion_intento_id",
                table: "respuestas_evaluacion",
                column: "intento_id");

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_evaluacion_opcion_respuesta_id",
                table: "respuestas_evaluacion",
                column: "opcion_respuesta_id");

            migrationBuilder.CreateIndex(
                name: "IX_respuestas_evaluacion_pregunta_id",
                table: "respuestas_evaluacion",
                column: "pregunta_id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_People_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contenidos_contenidos_documento_DocumentoContenidoId",
                table: "contenidos",
                column: "DocumentoContenidoId",
                principalTable: "contenidos_documento",
                principalColumn: "contenido_id");

            migrationBuilder.AddForeignKey(
                name: "FK_contenidos_contenidos_imagen_ImagenContenidoId",
                table: "contenidos",
                column: "ImagenContenidoId",
                principalTable: "contenidos_imagen",
                principalColumn: "contenido_id");

            migrationBuilder.AddForeignKey(
                name: "FK_contenidos_contenidos_video_VideoContenidoId",
                table: "contenidos",
                column: "VideoContenidoId",
                principalTable: "contenidos_video",
                principalColumn: "contenido_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_People_PersonId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_contenidos_lecciones_leccion_id",
                table: "contenidos");

            migrationBuilder.DropForeignKey(
                name: "FK_contenidos_contenidos_documento_DocumentoContenidoId",
                table: "contenidos");

            migrationBuilder.DropForeignKey(
                name: "FK_contenidos_contenidos_imagen_ImagenContenidoId",
                table: "contenidos");

            migrationBuilder.DropForeignKey(
                name: "FK_contenidos_contenidos_video_VideoContenidoId",
                table: "contenidos");

            migrationBuilder.DropTable(
                name: "calificaciones_items");

            migrationBuilder.DropTable(
                name: "inscripciones");

            migrationBuilder.DropTable(
                name: "nota_final_modulo");

            migrationBuilder.DropTable(
                name: "progreso_lecciones");

            migrationBuilder.DropTable(
                name: "respuestas_evaluacion");

            migrationBuilder.DropTable(
                name: "actividad_entregas");

            migrationBuilder.DropTable(
                name: "entregas");

            migrationBuilder.DropTable(
                name: "items_calificables");

            migrationBuilder.DropTable(
                name: "intentos_evaluacion");

            migrationBuilder.DropTable(
                name: "opciones_respuesta");

            migrationBuilder.DropTable(
                name: "preguntas");

            migrationBuilder.DropTable(
                name: "evaluaciones");

            migrationBuilder.DropTable(
                name: "lecciones");

            migrationBuilder.DropTable(
                name: "modulos");

            migrationBuilder.DropTable(
                name: "cursos");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "docentes");

            migrationBuilder.DropTable(
                name: "catalogos");

            migrationBuilder.DropTable(
                name: "contenidos_documento");

            migrationBuilder.DropTable(
                name: "contenidos_imagen");

            migrationBuilder.DropTable(
                name: "contenidos_video");

            migrationBuilder.DropTable(
                name: "contenidos");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "People");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_People_PersonId",
                table: "AspNetUsers",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
