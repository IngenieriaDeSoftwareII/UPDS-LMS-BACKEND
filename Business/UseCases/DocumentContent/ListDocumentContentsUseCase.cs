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
            ContentId = doc.ContentId,
            FileUrl = doc.FileUrl,
            Format = doc.Format.ToString(),
            SizeKb = doc.SizeKb,
            PageCount = doc.PageCount,

            Content = new ContentDto
            {
                Id = doc.Content.Id,
                LessonId = doc.Content.LessonId,
                Type = doc.Content.Type.ToString(),
                Title = doc.Content.Title,
                Order = doc.Content.Order,
                EntityStatus = doc.Content.EntityStatus,
                CreatedAt = doc.Content.CreatedAt,
                UpdatedAt = doc.Content.UpdatedAt,
                DeletedAt = doc.Content.DeletedAt
            }
        });

        return result;
    }
}