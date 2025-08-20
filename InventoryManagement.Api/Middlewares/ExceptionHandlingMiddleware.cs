using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.Api.Middlewares
{
    public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> _logger) : IMiddleware
    {
        //private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { ex.Message });
            }
            catch (UnAuthorizedException ex)
            {
                _logger.LogWarning(ex.Message);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { ex.Message });
            }
            catch (ForbidException ex)
            {
                _logger.LogWarning(ex.Message);

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { Message = "Accessing Forbidden" });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex.Message);

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new
                {
                    Message = "An unexpected error occurred.",
                    Details = ex.Message // Consider removing this in production for security reasons
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
