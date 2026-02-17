using AlterdataFinanceApi.Application.DTOs.Transaction;
using FluentValidation;

namespace AlterdataFinanceApi.Application.Validators;

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(200).WithMessage("Descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Valor deve ser maior que zero.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Data é obrigatória.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de transação inválido.");
    }
}
