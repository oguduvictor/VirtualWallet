using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Infrastructure.Persistence.Contexts;
using VirtualWallet.Infrastructure.Persistence.Repositories;
using VirtualWallet.Infrastructure.Persistence.Repository;

namespace VirtualWallet.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("VirtualWalletDB"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            #region Repositories
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<ITransactionRepositoryAsync, TransactionRepositoryAsync>();
            services.AddScoped<IWalletRepositoryAsync, WalletRepositoryAsync>();
            #endregion
        }
    }
}
