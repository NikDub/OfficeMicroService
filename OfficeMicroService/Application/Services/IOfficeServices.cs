using OfficeMicroService.Data.Enum;
using OfficeMicroService.Data.Models;
using OfficeMicroService.Data.Models.DTO;

namespace OfficeMicroService.Application.Services
{
    public interface IOfficeServices
    {
        Task<List<Office>> GetAllAsync();

        Task<Office> GetAsync(string id);

        Task<Office> CreateAsync(OfficeDTO model);

        Task<Office> UpdateAsync(string id, OfficeDTO model);

        Task RemoveAsync(string id);

        Task<Office> ChangeStatus(string id);
    }
}
