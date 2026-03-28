using AutoMapper;
using Business.DTOs.Responses;
using Data.Repositories.Interfaces;

namespace Business.UseCases.DocumentContent;

public class ListDocumentContentsUseCase
{
    private readonly IDocumentContentRepository _repository;
    private readonly IMapper _mapper;

    private const string Container = "documents";

    public ListDocumentContentsUseCase(IDocumentContentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DocumentContentDto>> ExecuteAsync()
    {
        var documents = await _repository.GetAllAsync();

        var result = documents.Select(doc => new DocumentContentDto
        {
            ContentId = doc.ContenidoId,
            FileUrl = doc.UrlArchivo,
            Format = doc.Formato.ToString(),
            SizeKb = doc.TamanoKb,
            PageCount = doc.NumPaginas,

            Content = new ContentDto
            {
                Id = doc.Contenido.Id,
                LessonId = doc.Contenido.LeccionId,
                Type = doc.Contenido.Tipo.ToString(),
                Title = doc.Contenido.Titulo,
                Order = doc.Contenido.Orden,
                EntityStatus = doc.Contenido.EntityStatus,
                CreatedAt = doc.Contenido.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = doc.Contenido.UpdatedAt ?? DateTime.UtcNow,
                DeletedAt = doc.Contenido.DeletedAt
            }
        });

        return result;
    }
}