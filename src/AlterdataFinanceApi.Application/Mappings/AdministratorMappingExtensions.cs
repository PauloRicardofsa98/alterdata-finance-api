using AlterdataFinanceApi.Application.DTOs.Administrator;
using AlterdataFinanceApi.Domain.Entities;

namespace AlterdataFinanceApi.Application.Mappings;

public static class AdministratorMappingExtensions
{
    public static AdministratorResponse ToResponse(this Administrator entity)
    {
        return new AdministratorResponse(
            Id: entity.Id,
            Name: entity.Name,
            Email: entity.Email,
            IsActive: entity.IsActive,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: entity.UpdatedAt
        );
    }

    public static Administrator ToEntity(this CreateAdministratorRequest request, string passwordHash)
    {
        return new Administrator
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash
        };
    }
}
