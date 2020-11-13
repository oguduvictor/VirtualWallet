using VirtualWallet.Domain.Common;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Domain.Entities
{
    public class Wallet : AuditableBaseEntity
    {
        public long AccountNumber { get; set; }
        public string WalletOwnerId { get; set; }
        public decimal AccountBalance { get; private set; }

        public void UpdateAccountBalance(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Credit) AccountBalance += transaction.Amount;
            else AccountBalance -= transaction.Amount;
        }
    }
}
