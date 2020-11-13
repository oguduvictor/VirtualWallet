using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VirtualWallet.Application.DTOs.Transaction;
using VirtualWallet.Application.Features.Wallets.Commands.CreateAccount;
using VirtualWallet.Application.Features.Wallets.Commands.Deposit;
using VirtualWallet.Application.Features.Wallets.Queries.GetTransactions;
using VirtualWallet.Application.Interfaces.Services;
using VirtualWallet.Application.Wrappers;

namespace VirtualWallet.WebApi.Controllers.v1
{
    /// <summary>
    /// Controller for managing wallet and transactions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : BaseApiController
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public WalletController(IAuthenticatedUserService authenticatedUserService)
        {
            _authenticatedUserService = authenticatedUserService;
        }

        /// <summary>
        /// Create Wallet and generate an account number.
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-wallet")]
        public async Task<ActionResult<Response<long>>> CreateWallet()
        {
            return Ok(await Mediator.Send(new CreateWalletCommand { UserId = _authenticatedUserService.UserId }));
        }

        /// <summary>
        /// Deposit money into user's wallet.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public async Task<ActionResult<Response<TransactionAlert>>> Deposit(DepositCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Withdraw from user's wallet.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Response<TransactionAlert>>> Withdraw(WithdrawCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Get Logged-in user's transactions.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetAllTransactionsViewModel>> GetTransactions([FromQuery] GetAllTransactionsParameter parameter)
        {
            return Ok(await Mediator.Send(new GetAllTransactionsQuery { PageNumber = parameter.PageNumber, PageSize = parameter.PageSize, TransactionType = parameter.TransactionType, From = parameter.From, To = parameter.To }));
        }
    }
}
