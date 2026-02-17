using AlterdataFinanceApi.Application.Interfaces;
using AlterdataFinanceApi.Application.Services;
using AlterdataFinanceApi.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AlterdataFinanceApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdministratorService, AdministratorService>();
        services.AddValidatorsFromAssemblyContaining<CreateAdministratorValidator>();
        return services;
    }
}
