using FluentValidation;
using InventoryManagement.Application.Account.Commands.ChangePassword;
using InventoryManagement.Application.Auth.Commands.Register;
using InventoryManagement.Application.Auth.Commands.ResetPassword;
using InventoryManagement.Application.LowStockAlerts.Command.CreateLowStockAlert;
using InventoryManagement.Application.Products.Commands.CreateProduct;
using InventoryManagement.Application.Products.Commands.UpdateProduct;
using InventoryManagement.Application.Transactions.Command.CancelTransaction;
using InventoryManagement.Application.Transactions.Command.CreateTransaction;
using InventoryManagement.Application.Transactions.Command.UpdateTransaction;
using InventoryManagement.Application.UserContextService;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace InventoryManagement.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

            // Register AutoMapper
            services.AddAutoMapper(applicationAssembly);

            // Register FluentValidation
            services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
            services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidator>();
            services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
            services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
            services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
            services.AddScoped<IValidator<CreateTransactionCommand>, CreateTransactionCommandValidator>();
            services.AddScoped<IValidator<UpdateTransactionCommand>, UpdateTransactionCommandValidator>();
            services.AddScoped<IValidator<CancelTransactionCommand>, CancelTransactionCommandValidator>();
            services.AddScoped<IValidator<CreateLowStockAlertCommand>, CreateLowStockAlertCommandValidator>();


            services.AddFluentValidationAutoValidation(options =>
            {
                //options.DisableBuiltInModelValidation = true;
                options.ValidationStrategy = ValidationStrategy.All; // Use FluentValidation for model validation
            });

            // Register IHttpContextAccessor
            services.AddScoped<IUserContext, UserContext>();
            services.AddHttpContextAccessor();
        }
    }
}
