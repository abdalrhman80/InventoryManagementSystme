using FluentValidation;

namespace InventoryManagement.Application.Transactions.Command.CancelTransaction
{
    public class CancelTransactionCommandValidator : AbstractValidator<CancelTransactionCommand>
    {
        public CancelTransactionCommandValidator()
        {
            RuleFor(x => x.Reason)
                .MaximumLength(500)
                .WithMessage("Cancellation reason cannot exceed 500 characters");
        }
    }
}
