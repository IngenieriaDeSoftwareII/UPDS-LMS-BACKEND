using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class LessonProfile : Profile
{
    public LessonProfile()
    {
        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<Lesson, LessonDto>();
    }
}