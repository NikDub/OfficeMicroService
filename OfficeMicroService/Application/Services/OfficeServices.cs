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
        var offices = await _repository.FindByCondition(office => true);
        return _mapper.Map<List<OfficeDto>>(offices);
    }

    public async Task<OfficeDto> GetAsync(string id)
    {
        var guid = Guid.Parse(id);
        var office = (await _repository.FindByCondition(office => office.Id == guid)).FirstOrDefault();
        return _mapper.Map<OfficeDto>(office);
    }

    public async Task<OfficeDto> CreateAsync(OfficeForChangeDto model)
    {
        if (model == null)
            return null;
        var mapModel = _mapper.Map<Office>(model);

        await _repository.CreateAsync(mapModel);

        return _mapper.Map<OfficeDto>(mapModel);
    }

    public async Task<OfficeDto> UpdateAsync(string id, OfficeForUpdateDto model)
    {
        var office = await GetAsync(id);
        if (office == null || model == null)
            return null;

        var mapModel = _mapper.Map<Office>(model);
        mapModel.Id = Guid.Parse(id);

        await _repository.UpdateAsync(mapModel);
        return _mapper.Map<OfficeDto>(mapModel);
    }

    public async Task RemoveAsync(string id)
    {
        var guid = Guid.Parse(id);
        await _repository.DeleteAsync(office => office.Id == guid);
    }

    public async Task<OfficeDto> ChangeStatusAsync(string id)
    {
        var office = await GetAsync(id);
        if (office == null)
            throw new NotFoundException("Office not found.");

        if (Enum.TryParse(office.Status, out OfficeStatus status))
        {
            office.Status = (status == OfficeStatus.Active ? OfficeStatus.Inactive : OfficeStatus.Active).ToString();

            var mapModel = _mapper.Map<Office>(office);
            await _repository.UpdateAsync(mapModel);
            return _mapper.Map<OfficeDto>(mapModel);
        }

        return null;
    }
}