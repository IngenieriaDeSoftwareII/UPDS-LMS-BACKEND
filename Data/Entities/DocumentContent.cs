using Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Table("contenidos_documento")]
public class DocumentContent
{
    [Key, ForeignKey("Contenido")]
    [Column("contenido_id")]
    public int ContenidoId { get; set; }

    [Required]
    [Column("url_archivo")]
    public string UrlArchivo { get; set; } = null!;

    [Required]
    [Column("formato")]
    public FormatDocument Formato { get; set; }

    [Column("tamano_kb")]
    public int? TamanoKb { get; set; }

    [Column("num_paginas")]
    public int? NumPaginas { get; set; }

    // relacion con contenido
    public Content Contenido { get; set; } = null!;
}
