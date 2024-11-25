using BankingManager.Database;
using BankingManager.Services;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BankingManager.Server.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services.AddScoped<DbContext, BankingManagerDbContext>()
                           .AddScoped(typeof(IEntityService<>), typeof(EntityService<>))
                           .AddScoped(typeof(IEntityExtendedService<>), typeof(EntityExtendedService<>))
                           .AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>))
                           .AddScoped(typeof(IBankAccountService), typeof(BankAccountService))
                           .AddScoped(typeof(IUtilityService), typeof(UtilityService));
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddScoped<AccountNumberValidator>()
                           .AddScoped<AmmountValidator>()
                           .AddScoped<CurrencyValidator>()
                           .AddScoped<IValidator<BankAccountAction>, BankAccountActionValidator>()
                           .AddScoped<IValidator<TransferAction>, BankAccountTransferValidator>()
                           .AddScoped<IValidator<BankAccountDTO>, BankAccountValidator>();

            //return services.AddValidatorsFromAssemblyContaining<>()
        }
    }
}
