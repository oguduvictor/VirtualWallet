using System;
using VirtualWallet.Application.Filters;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Application.Features.Wallets.Queries.GetTransactions
{
    public class GetAllTransactionsParameter : RequestParameter
    {
        public TransactionType? TransactionType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
