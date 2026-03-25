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
            entity.HasKey(e => e.ContentId);

            entity.HasOne(d => d.Content)
                  .WithOne()
                  .HasForeignKey<DocumentContent>(d => d.ContentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ImageContent 
        builder.Entity<ImageContent>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.HasOne(e => e.Content)
                  .WithOne()
                  .HasForeignKey<ImageContent>(e => e.ContentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        //VideoContent 
        builder.Entity<VideoContent>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.HasOne(e => e.Content)
                  .WithOne()
                  .HasForeignKey<VideoContent>(e => e.ContentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}