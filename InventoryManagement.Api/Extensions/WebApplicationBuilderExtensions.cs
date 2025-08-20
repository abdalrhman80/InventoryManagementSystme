using InventoryManagement.Api.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;

namespace InventoryManagement.Api.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        []
                    }
                });
            });

            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
            });

            builder.Services.AddScoped<ExceptionHandlingMiddleware>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            //// Configure API versioning
            //builder.Services.AddApiVersioning(options =>
            //{
            //    options.ReportApiVersions = true;
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.DefaultApiVersion = new ApiVersion(1, 0);
            //});
        }
    }
}
