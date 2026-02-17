using AlterdataFinanceApi.Application.DTOs.Auth;

namespace AlterdataFinanceApi.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
