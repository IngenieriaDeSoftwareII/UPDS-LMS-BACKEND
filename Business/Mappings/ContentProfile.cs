using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        // Content
        CreateMap<CreateContentDto, Content>();
        CreateMap<Content, ContentDto>();

        // Document
        CreateMap<CreateDocumentContentDto, DocumentContent>()
            .ForMember(dest => dest.Contenido, opt => opt.Ignore());
        CreateMap<DocumentContent, DocumentContentDto>();

        // Image
        CreateMap<CreateImageContentDto, ImageContent>()
            .ForMember(dest => dest.Contenido, opt => opt.Ignore()); 
        CreateMap<ImageContent, ImageContentDto>();

        // Video
        CreateMap<CreateVideoContentDto, VideoContent>()
            .ForMember(dest => dest.Contenido, opt => opt.Ignore()); 
        CreateMap<VideoContent, VideoContentDto>();
    }
}