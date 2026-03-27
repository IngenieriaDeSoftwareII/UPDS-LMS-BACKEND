using AutoMapper;
using Business.DTOs.Requests;
using Business.DTOs.Responses;
using Business.Results;
using Data.Entities;
using Data.Repositories.Interfaces;

namespace Business.UseCases.Catalogos;

public class CreateCatalogoUseCase(
    ICatalogoRepository repository,
    IMapper mapper)
{
    public async Task<Result<CatalogoDto>> ExecuteAsync(CreateCatalogoDto dto)
    {
        var entity = mapper.Map<Catalogo>(dto);
        var created = await repository.CreateAsync(entity);
        return Result<CatalogoDto>.Success(mapper.Map<CatalogoDto>(created));
    }
}

public class ListCatalogosUseCase(
    ICatalogoRepository repository,
    IMapper mapper)
{
    public async Task<Result<IEnumerable<CatalogoDto>>> ExecuteAsync()
    {
        var entities = await repository.GetAllAsync();
        return Result<IEnumerable<CatalogoDto>>.Success(mapper.Map<IEnumerable<CatalogoDto>>(entities));
    }
}

public class GetCatalogoByIdUseCase(
    ICatalogoRepository repository,
    IMapper mapper)
{
    public async Task<Result<CatalogoDto>> ExecuteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity == null) return Result<CatalogoDto>.Failure(["Catálogo no encontrado"]);
        
        return Result<CatalogoDto>.Success(mapper.Map<CatalogoDto>(entity));
    }
}