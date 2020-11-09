using System.Threading.Tasks;
using VirtualWallet.Broker.Commands;
using VirtualWallet.Broker.Events;

namespace VirtualWallet.Broker.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;
    }
}
