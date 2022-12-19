using MongoDB.Driver;
using OfficeMicroService.Data.Models;
using System.Linq.Expressions;

namespace OfficeMicroService.Data.Repository
{
    public interface IOfficeRepository
    {
        Task<List<Office>> FindByCondition(Expression<Func<Office, bool>> expression);
        Task CreateAsync(Office entity);
        Task UpdateAsync(Office entity);
        Task DeleteAsync(Expression<Func<Office, bool>> expression);
    }
}
