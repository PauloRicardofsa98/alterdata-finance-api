using AlterdataFinanceApi.Domain.Entities;

namespace AlterdataFinanceApi.Domain.Interfaces;

public interface IAdministratorRepository
{
    Task<Administrator?> GetByIdAsync(Guid id);
    Task<Administrator?> GetByEmailAsync(string email);
    Task<IEnumerable<Administrator>> GetAllAsync();
    Task<Administrator> AddAsync(Administrator administrator);
    Task<Administrator> UpdateAsync(Administrator administrator);
    Task DeleteAsync(Guid id);
}
