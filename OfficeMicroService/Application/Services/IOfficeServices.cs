using OfficeMicroService.Application.DTO;

namespace OfficeMicroService.Application.Services;

public interface IOfficeServices
{
    Task<List<OfficeDto>> GetAllAsync();

    Task<OfficeDto> GetAsync(Guid id);

    Task<OfficeDto> CreateAsync(OfficeForCreateDto model);

    Task<OfficeDto> UpdateAsync(Guid id, OfficeForUpdateDto model);

    Task ChangeStatusAsync(Guid id);
}