using AlterdataFinanceApi.Application.DTOs.Administrator;
using FluentValidation;

namespace AlterdataFinanceApi.Application.Validators;

public class CreateAdministratorValidator : AbstractValidator<CreateAdministratorRequest>
{
    public CreateAdministratorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.")
            .MaximumLength(200).WithMessage("E-mail deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres.");
    }
}
