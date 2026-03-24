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
}
