using MediatR;
using VirtualWallet.Application.DTOs.Transaction;
using VirtualWallet.Application.Wrappers;

namespace VirtualWallet.Application.Features.Wallets.Commands.Deposit
{
    public class DepositCommand : IRequest<Response<TransactionAlert>>
    {
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public long AccountNumber { get; set; }
    }
}
