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
                        ? InscriptionEstado.Activo
                        : Enum.Parse<InscriptionEstado>(v, true));
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.ToTable("progreso_lecciones");
        });
    }
}
