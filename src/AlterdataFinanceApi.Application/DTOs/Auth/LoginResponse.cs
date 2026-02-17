namespace AlterdataFinanceApi.Application.DTOs.Auth;

public record LoginResponse(
    string Token,
    string Name,
    string Email,
    DateTime ExpiresAt
);
