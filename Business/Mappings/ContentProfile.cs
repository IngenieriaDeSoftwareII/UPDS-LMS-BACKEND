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
        CreateMap<CreateContentDto, Content>()
            .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.LeccionId, opt => opt.MapFrom(src => src.LessonId))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Orden, opt => opt.MapFrom(src => src.Order));

        CreateMap<Content, ContentDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Titulo))
            .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LeccionId))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Tipo))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Orden));

        // Document
        CreateMap<CreateDocumentContentDto, DocumentContent>()
            .ForMember(dest => dest.ContenidoId, opt => opt.MapFrom(src => src.ContentId))
            .ForMember(dest => dest.UrlArchivo, opt => opt.MapFrom(src => src.FileUrl))
            .ForMember(dest => dest.Formato, opt => opt.MapFrom(src => src.Format))
            .ForMember(dest => dest.TamanoKb, opt => opt.MapFrom(src => src.SizeKb))
            .ForMember(dest => dest.NumPaginas, opt => opt.MapFrom(src => src.PageCount))
            .ForMember(dest => dest.Contenido, opt => opt.Ignore());

        CreateMap<DocumentContent, DocumentContentDto>();

        // Image
        CreateMap<CreateImageContentDto, ImageContent>()
            .ForMember(dest => dest.ContenidoId, opt => opt.MapFrom(src => src.ContentId))
            .ForMember(dest => dest.UrlImagen, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Formato, opt => opt.MapFrom(src => src.Format))
            .ForMember(dest => dest.AnchoPx, opt => opt.MapFrom(src => src.WidthPx))
            .ForMember(dest => dest.AltoPx, opt => opt.MapFrom(src => src.HeightPx))
            .ForMember(dest => dest.TextoAlternativo, opt => opt.MapFrom(src => src.AltText))
            .ForMember(dest => dest.TamanoKb, opt => opt.MapFrom(src => src.SizeKb))
            .ForMember(dest => dest.Contenido, opt => opt.Ignore());

        CreateMap<ImageContent, ImageContentDto>();

        // Video
        CreateMap<CreateVideoContentDto, VideoContent>()
            .ForMember(dest => dest.ContenidoId, opt => opt.MapFrom(src => src.ContentId))
            .ForMember(dest => dest.UrlVideo, opt => opt.MapFrom(src => src.VideoUrl))
            .ForMember(dest => dest.DuracionSeg, opt => opt.MapFrom(src => src.DurationSeconds))
            .ForMember(dest => dest.Contenido, opt => opt.Ignore());

        CreateMap<VideoContent, VideoContentDto>();
    }
}