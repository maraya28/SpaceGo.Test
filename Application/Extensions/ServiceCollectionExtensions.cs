using Application.Contracts;
using Application.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAplicationSetup(this IServiceCollection services)
        {
            services.AddScoped<ISlotService, SlotService>();
            services.AddScoped<IWallet, WalletService>();
            return services;
        }
    }
}
