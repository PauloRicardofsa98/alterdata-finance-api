namespace AlterdataFinanceApi.Application.DTOs.Administrator;

public record AdministratorResponse(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
