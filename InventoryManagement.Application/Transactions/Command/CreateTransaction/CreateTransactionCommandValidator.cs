using FluentValidation;

namespace InventoryManagement.Application.Transactions.Command.CreateTransaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("Product ID must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.TransactionType)
                .IsInEnum()
                .WithMessage("Invalid transaction type");
        }
    }
}
