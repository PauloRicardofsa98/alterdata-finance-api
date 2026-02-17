namespace AlterdataFinanceApi.Application.DTOs.Administrator;

public record CreateAdministratorRequest(
    string Name,
    string Email,
    string Password
);
