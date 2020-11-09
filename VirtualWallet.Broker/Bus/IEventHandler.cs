using System.Threading.Tasks;
using VirtualWallet.Broker.Events;

namespace VirtualWallet.Broker.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {

    }
}
