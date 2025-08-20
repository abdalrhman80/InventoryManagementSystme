using FluentValidation;

namespace InventoryManagement.Application.LowStockAlerts.Command.CreateLowStockAlert
{
    public class CreateLowStockAlertCommandValidator : AbstractValidator<CreateLowStockAlertCommand>
    {
        public CreateLowStockAlertCommandValidator()
        {
            RuleFor(a => a.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId must be great than 0");

            RuleFor(a => a.Threshold)
                .GreaterThan(0)
                .WithMessage("Threshold must be great than 0");
        }
    }
}
