using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<Inscription> Inscriptions { get; set; }
    public DbSet<LessonProgress> LessonProgresses { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Person)
                  .WithOne()
                  .HasForeignKey<User>(u => u.PersonId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);

            entity.HasMany(e => e.Inscripciones)
                  .WithOne(i => i.Cursos)
                  .HasForeignKey(i => i.CursoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Modulos)
                  .WithOne(m => m.Cursos)
                  .HasForeignKey(m => m.CursoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasMany(e => e.Inscripciones)
                  .WithOne(i => i.Usuarios)
                  .HasForeignKey(i => i.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);

            entity.HasMany(e => e.Lecciones)
                  .WithOne(l => l.Modulos)
                  .HasForeignKey(l => l.ModuloId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
        });

        modelBuilder.Entity<Inscription>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
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
    }
}
