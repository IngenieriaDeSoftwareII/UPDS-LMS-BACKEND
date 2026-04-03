using System.Globalization;
using Business.Helpers;
using Business.Results;
using Data.Enums;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Business.UseCases.StudentProgress;

public class GenerateCourseCertificatePdfUseCase(
    IUserRepository userRepository,
    IInscriptionRepository inscriptionRepository,
    ICourseRepository courseRepository,
    IEvaluationRepository evaluationRepository,
    IWebHostEnvironment webHostEnvironment)
{
    private static readonly CultureInfo Es = new("es-ES");

    private static readonly string[] BackgroundCandidates =
    [
        "certificado-fondo.png",
        "certificado-fondo.jpg",
        "certificado-fondo.jpeg",
    ];

    private const string TextoInstitucional = "#1A1F5F";

    public async Task<Result<byte[]>> ExecuteAsync(string userId, int cursoId)
    {
        var user = await userRepository.FindByIdWithPersonAsync(userId);
        if (user?.Person is null)
            return Result<byte[]>.Failure(["Usuario no encontrado."]);

        var personId = user.Person.Id;
        var inscription = await inscriptionRepository.GetByUserAndCourseAsync(personId, cursoId);
        if (inscription is null)
            return Result<byte[]>.Failure(["No hay inscripción para este curso."]);

        if (inscription.Estado != InscriptionEstate.Terminado || !inscription.FechaCompletado.HasValue)
            return Result<byte[]>.Failure(["El curso debe estar completado para emitir el certificado."]);

        var course = await courseRepository.GetByIdAsync(cursoId);
        if (course is null)
            return Result<byte[]>.Failure(["Curso no encontrado."]);

        var (evaluation, bestAttempt) =
            await evaluationRepository.GetCourseEvaluationAndBestAttemptAsync(cursoId, personId);
        if (evaluation is null)
            return Result<byte[]>.Failure(
                ["Este curso no tiene evaluación final configurada; no se puede emitir el certificado."]);

        if (bestAttempt is null)
            return Result<byte[]>.Failure(
                ["Debes rendir la evaluación final del curso y obtener al menos 51/100 para descargar el certificado."]);

        var notaSobre100 = CertificateExamRules.ComputeNotaSobre100(evaluation, bestAttempt.PuntajeObtenido);
        if (notaSobre100 < CertificateExamRules.MinNotaSobre100)
            return Result<byte[]>.Failure([
                $"No aprobaste la evaluación final. Tu nota es {notaSobre100:0.##}/100; se requiere nota mínima 51.",
            ]);

        QuestPDF.Settings.License = LicenseType.Community;

        var nombre = $"{user.Person.FirstName} {user.Person.LastName}".Trim();
        var fecha = inscription.FechaCompletado.Value.ToLocalTime();
        var fechaTexto = fecha.ToString("dd 'de' MMMM 'de' yyyy", Es);
        var bgPath = ResolveBackgroundPath();

        byte[]? bgBytes = null;
        if (bgPath is not null)
        {
            try
            {
                bgBytes = await File.ReadAllBytesAsync(bgPath);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure([$"No se pudo leer la plantilla del certificado: {ex.Message}"]);
            }
        }

        try
        {
            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(bgBytes is not null ? PageSizes.A4.Landscape() : PageSizes.A4);
                    page.Margin(0);

                    page.Content().Layers(layers =>
                    {
                        if (bgBytes is not null)
                        {
                            layers.Layer().Image(bgBytes).FitArea();
                        }

                        if (bgBytes is null)
                        {
                            layers.PrimaryLayer().Padding(48).Column(col =>
                            {
                                col.Spacing(20);
                                col.Item().AlignCenter().Text("Certificado de finalización")
                                    .FontSize(24).Bold();
                                col.Item().Height(24);
                                col.Item().AlignCenter().Text("Se certifica que").FontSize(12);
                                col.Item().AlignCenter().Text(nombre).FontSize(20).Bold();
                                col.Item().AlignCenter().Text("ha completado satisfactoriamente el curso").FontSize(12);
                                col.Item().AlignCenter().Text(course.Titulo).FontSize(16).Bold();
                                col.Item().Height(16);
                                col.Item().AlignCenter().Text($"Fecha de finalización: {fechaTexto}")
                                    .FontSize(11);
                            });
                        }
                        else
                        {
                            const float paddingSuperior = 292f;
                            const float huecoLineaAzul = 28f;
                            const float espacioCursoFecha = 12f;

                            layers.PrimaryLayer()
                                .PaddingTop(paddingSuperior)
                                .PaddingHorizontal(64f)
                                .Column(inner =>
                                {
                                    inner.Spacing(0);

                                    inner.Item().AlignCenter().Text(nombre)
                                        .FontSize(22)
                                        .Bold()
                                        .FontColor(TextoInstitucional);

                                    inner.Item().Height(huecoLineaAzul);

                                    inner.Item().Text(t =>
                                    {
                                        t.DefaultTextStyle(x => x.FontSize(15).FontColor(TextoInstitucional));
                                        t.AlignCenter();
                                        t.Span(course.Titulo);
                                    });

                                    inner.Item().Height(espacioCursoFecha);

                                    inner.Item().Text(t =>
                                    {
                                        t.DefaultTextStyle(x => x.FontSize(11).FontColor(TextoInstitucional));
                                        t.AlignCenter();
                                        t.Span(fechaTexto);
                                    });
                                });
                        }
                    });
                });
            }).GeneratePdf();

            return Result<byte[]>.Success(pdf);
        }
        catch (Exception ex)
        {
            return Result<byte[]>.Failure([$"Error al generar el PDF del certificado: {ex.Message}"]);
        }
    }

    private string? ResolveBackgroundPath()
    {
        var wwwroot = webHostEnvironment.WebRootPath;
        if (string.IsNullOrWhiteSpace(wwwroot))
            wwwroot = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");

        var dir = Path.Combine(wwwroot, "templates");
        foreach (var name in BackgroundCandidates)
        {
            var full = Path.Combine(dir, name);
            if (File.Exists(full))
                return full;
        }

        return null;
    }
}
