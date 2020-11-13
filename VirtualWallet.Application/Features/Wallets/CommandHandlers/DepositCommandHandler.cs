using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualWallet.Application.DTOs.Transaction;
using VirtualWallet.Application.Features.Wallets.Commands.Deposit;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Application.Wrappers;
using VirtualWallet.Domain.Entities;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Application.Features.Wallets.CommandHandlers
{
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Response<TransactionAlert>>
    {
        private readonly IWalletRepositoryAsync _walletRepositoryAsync;
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;

        public DepositCommandHandler(
            IWalletRepositoryAsync walletRepositoryAsync,
            ITransactionRepositoryAsync transactionRepositoryAsync)
        {
            _walletRepositoryAsync = walletRepositoryAsync;
            _transactionRepositoryAsync = transactionRepositoryAsync;
        }

        public async Task<Response<TransactionAlert>> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepositoryAsync.GetByAccountNumberAsync(request.AccountNumber);

            var transaction = new Transaction
            {
                Amount = request.Amount,
                Description = request.Description,
                Reference = Guid.NewGuid().ToString(),
                Type = TransactionType.Credit,
                WalletId = wallet.Id
            };

            var savedTransaction = await _transactionRepositoryAsync.AddAsync(transaction);

            wallet.UpdateAccountBalance(savedTransaction);

            await _walletRepositoryAsync.UpdateAsync(wallet);

            var transactionAlert = new TransactionAlert
            {
                AccountBalance = wallet.AccountBalance,
                Description = transaction.Description,
                TransactionReference = transaction.Reference
            };

            return new Response<TransactionAlert>(transactionAlert, $"You have successfully deposited {transaction.Amount} to account: {wallet.AccountNumber}.");
        }
    }
}
