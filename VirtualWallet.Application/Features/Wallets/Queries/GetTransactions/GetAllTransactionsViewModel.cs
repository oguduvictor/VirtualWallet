using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Application.Features.Wallets.Queries.GetTransactions
{
    public class GetAllTransactionsViewModel
    {
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public long AccountNumber { get; set; }
    }
}
