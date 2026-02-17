using AlterdataFinanceApi.Application.DTOs.Administrator;

namespace AlterdataFinanceApi.Application.Interfaces;

public interface IAdministratorService
{
    Task<AdministratorResponse> CreateAsync(CreateAdministratorRequest request);
    Task<AdministratorResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<AdministratorResponse>> GetAllAsync();
    Task<AdministratorResponse> UpdateAsync(Guid id, UpdateAdministratorRequest request);
    Task DeleteAsync(Guid id);
}
