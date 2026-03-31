namespace Business.DTOs.Requests
{
    public class CreateModuleDto
    {
        public int CursoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
    }
}