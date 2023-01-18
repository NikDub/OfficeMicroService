using System.Linq.Expressions;
using OfficeMicroService.Data.Models;

namespace OfficeMicroService.Data.Repository;

public interface IOfficeRepository
{
    Task<List<Office>> FindByCondition(Expression<Func<Office, bool>> expression);
    Task CreateAsync(Office entity);
    Task UpdateAsync(Office entity);
    Task DeleteAsync(Expression<Func<Office, bool>> expression);
}