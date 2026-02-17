using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlterdataFinanceApi.Application.DTOs.Auth;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AlterdataFinanceApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAdministratorRepository _repository;
    private readonly IValidator<LoginRequest> _validator;
    private readonly IConfiguration _configuration;

    public AuthService(
        IAdministratorRepository repository,
        IValidator<LoginRequest> validator,
        IConfiguration configuration)
    {
        _repository = repository;
        _validator = validator;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var admin = await _repository.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Credenciais inválidas.");

        if (!admin.IsActive)
            throw new UnauthorizedAccessException("Conta desativada.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            throw new UnauthorizedAccessException("Credenciais inválidas.");

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var expiresInHours = int.Parse(jwtSettings["ExpiresInHours"] ?? "24");
        var expiresAt = DateTime.UtcNow.AddHours(expiresInHours);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, admin.Email),
            new Claim(JwtRegisteredClaimNames.Name, admin.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse(tokenString, admin.Name, admin.Email, expiresAt);
    }
}
