using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtualWallet.Application.Features.Wallets.Commands.CreateAccount;
using VirtualWallet.Application.Interfaces.Repositories;
using VirtualWallet.Application.Wrappers;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Application.Features.Wallets.CommandHandlers
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Response<long>>
    {
        private readonly IWalletRepositoryAsync _walletRepositoryAsync;

        public CreateWalletCommandHandler(IWalletRepositoryAsync walletRepositoryAsync)
        {
            _walletRepositoryAsync = walletRepositoryAsync;
        }

        public async Task<Response<long>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = new Wallet
            {
                WalletOwnerId = request.UserId,
                AccountNumber = await GenerateAccountNumber()
            };

            await _walletRepositoryAsync.AddAsync(wallet);

            return new Response<long>(wallet.AccountNumber, $"New wallet created with account number '{wallet.AccountNumber}'.");
        }

        private async Task<long> GenerateAccountNumber()
        {
            var random = new Random();

            var myNumber = random.Next(0, int.MaxValue);

            var accountNumber = long.Parse(string.Format("{0:D10}", myNumber));

            if (await _walletRepositoryAsync.AccountNumberExistsAsync(accountNumber))
            {
                return await GenerateAccountNumber();
            }

            return accountNumber;
        }
    }
}
