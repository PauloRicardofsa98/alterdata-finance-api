using AlterdataFinanceApi.Application.DTOs.Administrator;
using FluentValidation;

namespace AlterdataFinanceApi.Application.Validators;

public class UpdateAdministratorValidator : AbstractValidator<UpdateAdministratorRequest>
{
    public UpdateAdministratorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.")
            .MaximumLength(200).WithMessage("E-mail deve ter no máximo 200 caracteres.");
    }
}
