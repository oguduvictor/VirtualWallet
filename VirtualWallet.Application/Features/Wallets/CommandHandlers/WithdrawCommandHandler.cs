using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualWallet.Application.DTOs.Transaction;
using VirtualWallet.Application.Exceptions;
using VirtualWallet.Application.Features.Wallets.Commands.Deposit;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Application.Interfaces.Services;
using VirtualWallet.Application.Wrappers;
using VirtualWallet.Domain.Entities;
using VirtualWallet.Domain.Enums;

namespace VirtualWallet.Application.Features.Wallets.CommandHandlers
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Response<TransactionAlert>>
    {
        private readonly IWalletRepositoryAsync _walletRepositoryAsync;
        private readonly ITransactionRepositoryAsync _transactionRepositoryAsync;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public WithdrawCommandHandler(
            IWalletRepositoryAsync walletRepositoryAsync,
            ITransactionRepositoryAsync transactionRepositoryAsync,
            IAuthenticatedUserService authenticatedUserService)
        {
            _walletRepositoryAsync = walletRepositoryAsync;
            _transactionRepositoryAsync = transactionRepositoryAsync;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<TransactionAlert>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var loggedInUserId = _authenticatedUserService.UserId;

            if (string.IsNullOrEmpty(loggedInUserId))
            {
                throw new ApiException("You must be logged in to withdraw from your account");
            }

            var wallet = await _walletRepositoryAsync.GetByWalletOwnerIdAsync(loggedInUserId);

            if (wallet.AccountBalance <= request.Amount)
            {
                throw new ApiException("You are trying to withdraw more than what you have. Please leave some for next time.");
            }

            var transaction = new Transaction
            {
                Amount = request.Amount,
                Description = request.Description,
                Reference = Guid.NewGuid().ToString(),
                Type = TransactionType.Debit,
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

            return new Response<TransactionAlert>(transactionAlert, $"You have successfully withdrawn {transaction.Amount} from your account: {wallet.AccountNumber}.");
        }
    }
}
