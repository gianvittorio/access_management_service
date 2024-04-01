using AccessManagementService.Service.AccessManagement.Exceptions.PasswordValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace AccessManagementService.Web.ExceptionHandlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is PasswordValidationException passwordValidationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            
            var responseBody = new
            {
                Message = "Password is invalid. Please make sure it has at least 8 characters, a letter, a digit, and a symbol.",
                Reason = passwordValidationException.PasswordValidationResult
            };
            await httpContext.Response.WriteAsJsonAsync(responseBody, cancellationToken: cancellationToken);
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Message = "An error occurred while processing your request." }, cancellationToken: cancellationToken);
    
        }
        
        return true;
    }
}