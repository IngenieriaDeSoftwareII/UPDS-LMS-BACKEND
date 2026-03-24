using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Person> People { get; set; }

    // grupo 2 contenidos
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<ImageContent> ImageContents { get; set; }
    public DbSet<DocumentContent> DocumentContents { get; set; }
    public DbSet<VideoContent> VideoContents { get; set; }

    //hacer la relación entre Content y sus subtipos (ImageContent, DocumentContent, VideoContent)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configuración para DocumentContent
        builder.Entity<DocumentContent>(entity =>
        {
            entity.HasKey(e => e.ContentId);
            entity.HasOne(d => d.Content)
                  .WithOne() // Ajusta si Content tiene propiedad de navegación
                  .HasForeignKey<DocumentContent>(d => d.ContentId);
        });

        // Repitir lo mismo para ImageContent y VideoContent
        builder.Entity<ImageContent>().HasKey(e => e.ContentId);
        builder.Entity<VideoContent>().HasKey(e => e.ContentId);
    }
}
