using InventoryManagement.Api.Extensions;
using InventoryManagement.Api.Middlewares;
using InventoryManagement.Application.Extensions;
using InventoryManagement.Infrastructure.DatabaseInitializer;
using InventoryManagement.Infrastructure.Extensions;
using Serilog;

namespace InventoryManagement.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI Container
            builder.AddPresentation();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            #endregion

            var app = builder.Build();

            #region Initialize Database
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
            #endregion

            #region Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
