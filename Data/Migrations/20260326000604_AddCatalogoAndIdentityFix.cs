using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCatalogoAndIdentityFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogoId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityStatus = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CatalogoId",
                table: "Categorias",
                column: "CatalogoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Catalogos_CatalogoId",
                table: "Categorias",
                column: "CatalogoId",
                principalTable: "Catalogos",
                principalColumn: "Id");

            // Deshabilitar la caché de Identity para evitar saltos en los IDs de Docentes y otras tablas
            migrationBuilder.Sql("ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF;", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Catalogos_CatalogoId",
                table: "Categorias");

            migrationBuilder.DropTable(
                name: "Catalogos");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_CatalogoId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CatalogoId",
                table: "Categorias");

            migrationBuilder.Sql("ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;", suppressTransaction: true);
        }
    }
}
