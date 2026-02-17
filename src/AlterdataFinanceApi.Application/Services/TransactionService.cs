using AlterdataFinanceApi.Application.DTOs.Transaction;
using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Application.Mappings;
using AlterdataFinanceApi.Domain.Interfaces;
using FluentValidation;

namespace AlterdataFinanceApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IValidator<CreateTransactionRequest> _createValidator;
    private readonly IValidator<UpdateTransactionRequest> _updateValidator;

    public TransactionService(
        ITransactionRepository repository,
        IValidator<CreateTransactionRequest> createValidator,
        IValidator<UpdateTransactionRequest> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<TransactionResponse> CreateAsync(CreateTransactionRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = request.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToResponse();
    }

    public async Task<TransactionResponse> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Transação não encontrada.");

        return entity.ToResponse();
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToResponse());
    }

    public async Task<TransactionResponse> UpdateAsync(Guid id, UpdateTransactionRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Transação não encontrada.");

        entity.Description = request.Description;
        entity.Amount = request.Amount;
        entity.Date = request.Date;
        entity.Category = request.Category;
        entity.Type = request.Type;

        var updated = await _repository.UpdateAsync(entity);
        return updated.ToResponse();
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Transação não encontrada.");

        await _repository.DeleteAsync(id);
    }
}
