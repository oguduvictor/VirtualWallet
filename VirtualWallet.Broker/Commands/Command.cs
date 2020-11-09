using System;
using VirtualWallet.Broker.Events;

namespace VirtualWallet.Broker.Commands
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }
    }
}
