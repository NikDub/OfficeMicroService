using MongoDB.Bson;
using OfficeMicroService.Data.Models;

namespace OfficeMicroService.Data.Repository
{
    public interface IOffiseRepository
    {
        Task UpdateServiceAsync(Office office);
        Task CreateOfficeAsync(Office office);
        Task DeleteOfficeAsync(Office office);
        Task<List<Office>> GetAllOfficesAsync();
        Task<Office> GetOfficeAsync(ObjectId officeId);
    }
}
