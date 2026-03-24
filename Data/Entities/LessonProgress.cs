using Data.Enums; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace Data.Entities
{
    [Table("progreso_lecciones")]
    public class LessonProgress
    {
        [Key]
        public int id { get; set; }

        public int usuario_id { get; set; }
        public int leccion_id { get; set; }
        public bool completado {  get; set; } = false;
        public decimal posicion_actual { get; set; }
        public DateTime? fecha_completado { get; set; }
        public short EntityStatus { get; set; } = 1;

        public User User { get; set; } = null!;
        public Lesson Lesson { get; set; } = null!;
    }
}
