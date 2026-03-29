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
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Es crucial mantener base.OnModelCreating para que Identity funcione correctamente
        base.OnModelCreating(builder);
        
        // Aquí puedes agregar configuraciones de Fluent API si las necesitas más adelante
    }
}