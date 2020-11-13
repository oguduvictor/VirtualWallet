using MediatR;
using VirtualWallet.Application.Wrappers;

namespace VirtualWallet.Application.Features.Wallets.Commands.CreateAccount
{
    public class CreateWalletCommand : IRequest<Response<long>>
    {
        public string UserId { get; set; }
    }
}
