using Infrastructure.Contracts;
using Infrastructure.Implementations;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureSetup(this IServiceCollection services)
        {
            services.AddDbContext<SpaceGoDbContext>(dbContext => dbContext.UseInMemoryDatabase("SpaceGo"));
            services.AddScoped<IWalletStore, WalletStore>();
            services.AddScoped<ISlotStore, SlotStore>();
            return services;
        }
    }
}
