using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Category
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required, MaxLength(100)]
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public short EntityStatus { get; set; } = 1;

    public int? CatalogId { get; set; }
    public Catalog? Catalog { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}