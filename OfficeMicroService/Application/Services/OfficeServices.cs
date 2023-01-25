using AutoMapper;
using OfficeMicroService.Application.DTO;
using OfficeMicroService.Application.Exceptions;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Repository;

namespace OfficeMicroService.Application.Services;

public class OfficeServices : IOfficeServices
{
    private readonly IMapper _mapper;
    private readonly IOfficeRepository _repository;

    public OfficeServices(IOfficeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OfficeDto>> GetAllAsync()
    {
        var offices = await _repository.FindAllAsync();
        return _mapper.Map<List<OfficeDto>>(offices);
    }

    public async Task<OfficeDto> GetAsync(Guid id)
    {
        var office = (await _repository.FindByConditionAsync(office => office.Id == id)).FirstOrDefault();
        return _mapper.Map<OfficeDto>(office);
    }

    public async Task<OfficeDto> CreateAsync(OfficeForCreateDto model)
    {
        if (model == null)
            return null;
        var mapModel = _mapper.Map<Office>(model);

        await _repository.CreateAsync(mapModel);

        return _mapper.Map<OfficeDto>(mapModel);
    }

    public async Task<OfficeDto> UpdateAsync(Guid id, OfficeForUpdateDto model)
    {
        var office = await GetAsync(id);
        if (office == null || model == null)
            return null;

        var mapModel = _mapper.Map<Office>(model);
        mapModel.Id = id;

        await _repository.UpdateAsync(mapModel);
        return _mapper.Map<OfficeDto>(mapModel);
    }

    public async Task ChangeStatusAsync(Guid id)
    {
        var office = await GetAsync(id);
        if (office == null)
            throw new NotFoundException("Office not found.");

        office.Status = office.Status == OfficeStatus.Active ? OfficeStatus.Inactive : OfficeStatus.Active;

        var mapModel = _mapper.Map<Office>(office);
        await _repository.UpdateAsync(mapModel);
    }
}