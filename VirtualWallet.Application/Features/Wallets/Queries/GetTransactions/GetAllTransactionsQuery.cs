using MediatR;
using System;
using System.Collections.Generic;
using VirtualWallet.Application.Wrappers;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Application.Features.Wallets.Queries.GetTransactions
{
    public class GetAllTransactionsQuery : IRequest<PagedResponse<IEnumerable<GetAllTransactionsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
