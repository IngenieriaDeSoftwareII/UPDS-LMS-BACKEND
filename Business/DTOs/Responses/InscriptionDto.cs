namespace Business.DTOs.Responses
{
    public class InscriptionDto
    {
        public int Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? FechaCompletado { get; set; }
        public DateTime CreatedAt { get; set; }

        public CourseDto Curso { get; set; } = null!;
    }
}
