using OfficeMicroService.Application.DTO;

namespace OfficeMicroService.Application.Services;

public interface IOfficeServices
{
    Task<List<OfficeDto>> GetAllAsync();

    Task<OfficeDto> GetAsync(string id);

    Task<OfficeDto> CreateAsync(OfficeForCreateDto model);

    Task<OfficeDto> UpdateAsync(string id, OfficeForUpdateDto model);

    Task ChangeStatusAsync(string id);
}