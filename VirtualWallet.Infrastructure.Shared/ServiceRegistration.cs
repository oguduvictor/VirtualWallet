using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualWallet.Application.Interfaces.Services;
using VirtualWallet.Domain.Settings;
using VirtualWallet.Infrastructure.Shared.Services;

namespace VirtualWallet.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
