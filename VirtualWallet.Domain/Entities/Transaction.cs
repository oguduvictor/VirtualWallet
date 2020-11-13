using System;
using VirtualWallet.Domain.Common;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Domain.Entities
{
    public class Transaction : AuditableBaseEntity
    {
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public Wallet Wallet { get; set; }
        public Guid WalletId { get; set; }
    }
}
