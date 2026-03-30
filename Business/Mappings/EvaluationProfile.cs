using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Data.Entities;

namespace Business.Mappings;

public class EvaluationProfile : Profile
{
    public EvaluationProfile()
    {
        CreateMap<CreateEvaluationDto, Evaluation>();
        CreateMap<Evaluation, EvaluationDto>();

        CreateMap<AddEvaluationQuestionDto, Question>()
            .ForMember(d => d.OpcionesRespuesta, o => o.Ignore());

        CreateMap<AddAnswerOptionDto, AnswerOption>();

        CreateMap<Question, EvaluationQuestionDto>()
            .ForMember(d => d.Opciones, o => o.MapFrom(s => s.OpcionesRespuesta));

        CreateMap<AnswerOption, EvaluationAnswerOptionDto>();
    }
}

