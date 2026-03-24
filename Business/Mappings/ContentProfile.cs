using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        CreateMap<CreateContentDto, Content>();
        CreateMap<Content, ContentDto>();
        CreateMap<CreateDocumentContentDto, DocumentContent>();
        CreateMap<DocumentContent, DocumentContentDto>();
        CreateMap<CreateImageContentDto, ImageContent>();
        CreateMap<ImageContent, ImageContentDto>();
        CreateMap<CreateVideoContentDto, VideoContent>();
        CreateMap<VideoContent, VideoContentDto>();
    }
}