using OfficeMicroService.Application.DTO;
using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;

namespace OfficeMicroService.Application.Services
{
    public interface IOfficeServices
    {
        Task<List<OfficeDTO>> GetAllAsync();

        Task<OfficeDTO> GetAsync(string id);

        Task<OfficeDTO> CreateAsync(OfficeForChangeDTO model);

        Task<OfficeDTO> UpdateAsync(string id, OfficeForUpdateDTO model);

        Task RemoveAsync(string id);

        Task<OfficeDTO> ChangeStatusAsync(string id);
    }
}
