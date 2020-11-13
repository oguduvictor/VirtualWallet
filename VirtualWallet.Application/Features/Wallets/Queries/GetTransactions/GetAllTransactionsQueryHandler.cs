using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Application.Wrappers;

namespace VirtualWallet.Application.Features.Wallets.Queries.GetTransactions
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, PagedResponse<IEnumerable<GetAllTransactionsViewModel>>>
    {
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;

        public GetAllTransactionsQueryHandler(ITransactionRepositoryAsync transactionRepositoryAsync)
        {
            _transactionRepositoryAsync = transactionRepositoryAsync;
        }

        public async Task<PagedResponse<IEnumerable<GetAllTransactionsViewModel>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepositoryAsync.GetPagedReponseAsync(
                request.PageNumber,
                request.PageSize,
                x =>
                    request.TransactionType != null ? x.Type == request.TransactionType : true
                    &&
                    x.Created >= request.From && x.Created <= request.To,
                x => x.Wallet);

            var transactionsVM = transactions.Select(x => new GetAllTransactionsViewModel
            {
                AccountNumber = x.Wallet.AccountNumber,
                Amount = x.Amount,
                Type = x.Type,
                Description = x.Description,
                Reference = x.Reference
            });

            return new PagedResponse<IEnumerable<GetAllTransactionsViewModel>>(transactionsVM, request.PageNumber, request.PageSize);
        }
    }
}
