using FluentValidation;

namespace InventoryManagement.Application.Transactions.Command.UpdateTransaction
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator()
        {
            RuleFor(x => x.NewQuantity)
                .GreaterThan(0)
                .When(x => x.NewQuantity.HasValue)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .When(x => x.TransactionType.HasValue)
                .WithMessage("Invalid transaction type");

            RuleFor(x => x)
                .Must(x => x.TransactionType.HasValue || x.NewQuantity.HasValue)
                .WithMessage("At least one field must be provided for update");
        }
    }
}
