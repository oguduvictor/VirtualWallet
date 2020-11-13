using FluentValidation;

namespace VirtualWallet.Application.Features.Wallets.Commands.Deposit
{
    public class DepositCommandValidator : AbstractValidator<DepositCommand>
    {
        public DepositCommandValidator()
        {
            RuleFor(p => p.AccountNumber)
                .NotEmpty()
                .GreaterThanOrEqualTo(1000000000)
                .LessThanOrEqualTo(9999999999)
                .WithMessage("Account number is not valid");

            RuleFor(p => p.Amount)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Only positive numbers allowed.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Describe the deposit reason.");

        }
    }
}
