using Data.Entities;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 🔥 DocumentContent se agrego cascade
        builder.Entity<DocumentContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(d => d.Contenido)
                  .WithOne()
                  .HasForeignKey<DocumentContent>(d => d.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ImageContent 
        builder.Entity<ImageContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(e => e.Contenido)
                  .WithOne()
                  .HasForeignKey<ImageContent>(e => e.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        //VideoContent 
        builder.Entity<VideoContent>(entity =>
        {
            entity.HasKey(e => e.ContenidoId);

            entity.HasOne(e => e.Contenido)
                  .WithOne()
                  .HasForeignKey<VideoContent>(e => e.ContenidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}