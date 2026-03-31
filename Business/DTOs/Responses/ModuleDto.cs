namespace Business.DTOs.Responses
{
    public class ModuleDto
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
    }
}