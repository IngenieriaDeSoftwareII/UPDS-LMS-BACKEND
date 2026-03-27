using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Person> People { get; set; }
    
    // Auth Entities
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // LMS Entities
    public DbSet<Catalogo> Catalogos { get; set; }
    public DbSet<Docente> Docentes { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Es crucial mantener base.OnModelCreating para que Identity funcione correctamente
        base.OnModelCreating(builder);
        
        // Aquí puedes agregar configuraciones de Fluent API si las necesitas más adelante
    }
}