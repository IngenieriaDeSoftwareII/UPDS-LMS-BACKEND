namespace Business.DTOs.Responses;

public class StudentCourseLearningDto
{
    public int CursoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Nivel { get; set; }
    public string? ImagenUrl { get; set; }
    public int DuracionTotalMin { get; set; }
    public string? DocenteNombre { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;

    public bool Inscrito { get; set; }
    public int? InscripcionId { get; set; }
    public string? EstadoInscripcion { get; set; }

    public int LeccionesTotales { get; set; }
    public int LeccionesCompletadas { get; set; }
    public int ProgresoPorcentaje { get; set; }

   public bool CursoCompletado { get; set; }

    public bool TieneEvaluacionFinal { get; set; }
    public decimal? NotaEvaluacionSobre100 { get; set; }
    public bool AprobadoEvaluacionFinal { get; set; }
    public bool PuedeDescargarCertificado { get; set; }
    public string? MensajeCertificado { get; set; }

    public IReadOnlyList<StudentModuleLearningDto> Modulos { get; set; } = [];
}

public class StudentModuleLearningDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Orden { get; set; }
    public IReadOnlyList<StudentLessonLearningDto> Lecciones { get; set; } = [];
}

public class StudentLessonLearningDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int? Orden { get; set; }
    public bool Completada { get; set; }
}
