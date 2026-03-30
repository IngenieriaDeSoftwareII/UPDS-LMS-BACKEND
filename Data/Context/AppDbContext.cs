using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Person> People { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<ImageContent> ImageContents { get; set; }
    public DbSet<DocumentContent> DocumentContents { get; set; }
    public DbSet<VideoContent> VideoContents { get; set; }
    public DbSet<Inscription> Inscriptions { get; set; }
    public DbSet<LessonProgress> LessonProgresses { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Evaluation> Evaluations { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<EvaluationAttempt> EvaluationAttempts { get; set; }
    public DbSet<EvaluationAnswer> EvaluationAnswers { get; set; }

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<ActivitySubmission> ActivitySubmissions { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<GradableItem> GradableItems { get; set; }
    public DbSet<ItemGrade> ItemGrades { get; set; }
    public DbSet<ModuleFinalGrade> ModuleFinalGrades { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)

    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Person)
                  .WithMany(p => p.Users)
                  .HasForeignKey(u => u.PersonId)
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

            entity.Property(e => e.IsActive).HasDefaultValue(true);
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

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.IntentosPermitidos).HasDefaultValue(1);

            entity.HasOne(e => e.Cursos)
                  .WithMany()
                  .HasForeignKey(e => e.CursoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Leccion)
                  .WithMany(l => l.Evaluaciones)
                  .HasForeignKey(e => e.LeccionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Preguntas)
                  .WithOne(p => p.Evaluaciones)
                  .HasForeignKey(p => p.EvaluacionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Intentos)
                  .WithOne(i => i.Evaluaciones)
                  .HasForeignKey(i => i.EvaluacionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.Puntos).HasDefaultValue(1);
            entity.Property(e => e.Orden).HasDefaultValue(1);

            entity.HasMany(e => e.OpcionesRespuesta)
                  .WithOne(o => o.Preguntas)
                  .HasForeignKey(o => o.PreguntaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.Orden).HasDefaultValue(1);
            entity.Property(e => e.EsCorrecta).HasDefaultValue(false);
        });

        modelBuilder.Entity<EvaluationAttempt>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.NumeroIntento).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasMany(e => e.Respuestas)
                  .WithOne(r => r.Intentos)
                  .HasForeignKey(r => r.IntentoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EvaluationAnswer>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);

            entity.HasOne(e => e.Preguntas)
                  .WithMany(p => p.Respuestas)
                  .HasForeignKey(e => e.PreguntaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OpcionesRespuesta)
                  .WithMany(o => o.RespuestasSeleccionadas)
                  .HasForeignKey(e => e.OpcionRespuestaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Inscription>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.Estado)
                  .HasMaxLength(50)
                  .HasConversion(
                      v => v.ToString(),
                      v => string.IsNullOrEmpty(v)
                          ? InscriptionEstate.Activo
                          : Enum.Parse<InscriptionEstate>(v, true));
        });

        modelBuilder.Entity<LessonProgress>(entity =>
        {
            entity.ToTable("progreso_lecciones");
        });
        // 🔥 DocumentContent se agrego cascade
        modelBuilder.Entity<DocumentContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(d => d.Contenido)
                  .WithOne()
                  .HasForeignKey<DocumentContent>(d => d.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ImageContent 
        modelBuilder.Entity<ImageContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(e => e.Contenido)
                  .WithOne()
                  .HasForeignKey<ImageContent>(e => e.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        //VideoContent 
        modelBuilder.Entity<VideoContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(e => e.Contenido)
                  .WithOne()
                  .HasForeignKey<VideoContent>(e => e.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // GradableItem
        modelBuilder.Entity<GradableItem>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
            entity.Property(e => e.Orden).HasDefaultValue(1);

            entity.HasOne(e => e.Module)
                  .WithMany(m => m.ItemsCalificables)
                  .HasForeignKey(e => e.ModuloId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ItemGrade
        modelBuilder.Entity<ItemGrade>(entity =>
        {
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);

            entity.HasOne(e => e.Submission)
                  .WithMany()
                  .HasForeignKey(e => e.EntregaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ActivitySubmission)
                  .WithMany()
                  .HasForeignKey(e => e.ActividadEntregaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ModuleFinalGrade
        modelBuilder.Entity<ModuleFinalGrade>(entity =>
        {
            entity.Property(e => e.ItemsCalificados).HasDefaultValue(0);
            entity.Property(e => e.ItemsTotales).HasDefaultValue(0);
            entity.Property(e => e.EntityStatus).HasDefaultValue((short)1);
        });
    }

}

