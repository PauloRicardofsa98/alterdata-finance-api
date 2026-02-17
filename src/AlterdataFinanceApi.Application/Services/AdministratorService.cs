using AlterdataFinanceApi.Application.DTOs.Administrator;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Application.Mappings;
using AlterdataFinanceApi.Domain.Interfaces;
using FluentValidation;

namespace AlterdataFinanceApi.Application.Services;

public class AdministratorService : IAdministratorService
{
    private readonly IAdministratorRepository _repository;
    private readonly IValidator<CreateAdministratorRequest> _createValidator;
    private readonly IValidator<UpdateAdministratorRequest> _updateValidator;

    public AdministratorService(
        IAdministratorRepository repository,
        IValidator<CreateAdministratorRequest> createValidator,
        IValidator<UpdateAdministratorRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<AdministratorResponse> CreateAsync(CreateAdministratorRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _repository.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new InvalidOperationException("Já existe um administrador com este e-mail.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var entity = request.ToEntity(passwordHash);

        var created = await _repository.AddAsync(entity);
        return created.ToResponse();
    }

    public async Task<AdministratorResponse> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Administrador não encontrado.");

        return entity.ToResponse();
    }

    public async Task<IEnumerable<AdministratorResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToResponse());
    }

    public async Task<AdministratorResponse> UpdateAsync(Guid id, UpdateAdministratorRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Administrador não encontrado.");

        if (!string.Equals(entity.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repository.GetByEmailAsync(request.Email);
            if (existing is not null)
                throw new InvalidOperationException("Já existe um administrador com este e-mail.");
        }

        entity.UpdateProfile(request.Name, request.Email);

        var updated = await _repository.UpdateAsync(entity);
        return updated.ToResponse();
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Administrador não encontrado.");

        await _repository.DeleteAsync(id);
    }
}
