using FluentValidation;

namespace VirtualWallet.Application.Features.Wallets.Commands.Deposit
{
    public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
    {
        public WithdrawCommandValidator()
        {
            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Describe the withdrawal reason.");

            RuleFor(p => p.Amount)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Only positive numbers allowed.");
        }
    }
}
