using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VirtualWallet.Broker.Bus;

namespace VirtualWallet.Broker
{
    public static class ServiceExtensions
    {
        public static void AddBrokerServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopeFactory);
            });
        }
    }
}
