using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    [Table("inscripciones")] 
    public class Inscription
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("curso_id")]
        public int CursoId { get; set; }

        [Column("fecha_completado")]
        public DateTime? FechaCompletado { get; set; }

        [Column("estado")]
        [StringLength(50)]
        public string Estado { get; set; } = "activo";

        [Column("entity_status")]
        public short EntityStatus { get; set; } = 1;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        
        [ForeignKey("UsuarioId")]
        public virtual AspNetUser Usuario { get; set; }

        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; }
    }
}