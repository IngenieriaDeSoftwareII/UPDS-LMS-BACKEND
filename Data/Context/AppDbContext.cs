using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Person> People { get; set; }

    public DbSet<Inscription> Inscriptions { get; set; }

    public DbSet<LessonProgress> LessonProgresses { get; set; }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Inscription>(entity =>
        {
            entity.ToTable("inscripciones");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasConversion(
                    v => v.ToString().ToLowerInvariant(),
                    v => string.IsNullOrEmpty(v)
                        ? InscriptionEstate.Activo
                        : Enum.Parse<InscriptionEstate>(v, true));
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.ToTable("progreso_lecciones");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("cursos");
        });
    }
}
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
