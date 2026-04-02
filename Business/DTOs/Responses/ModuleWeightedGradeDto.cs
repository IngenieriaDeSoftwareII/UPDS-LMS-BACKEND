namespace Business.DTOs.Responses;

public class ModuleWeightedGradeDto
{
    public int ModuloId { get; set; }
    public string TituloModulo { get; set; } = string.Empty;
    public int Orden { get; set; }
    public decimal? NotaPonderada { get; set; }
    public decimal PonderacionTotal { get; set; }
    public int ItemsConEvaluacion { get; set; }
    public int ItemsCalificados { get; set; }
}
