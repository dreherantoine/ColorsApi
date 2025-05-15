using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ColorsApi.Configurations;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddErrorHandling(this WebApplicationBuilder builder)
    {
        // Gestion de .net standard pour permettre générer des ProblemDetails
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        // Gestion customisée des erreurs de type Validation
        builder.Services.AddExceptionHandler<FluentValidationExceptionHandler>();

        // Attrape toute les erreurs restantes
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }
}

public class FluentValidationExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        // On force le Status code à 400
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        // Un maximum d'information car il s'agit d'une erreur client
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = "One or more validation errors occurred",
                Status = StatusCodes.Status400BadRequest
            }
        };

        var errors = validationException.Errors
            .ToDictionary(
                error => error.PropertyName,
                error => error.ErrorMessage
            );

        context.ProblemDetails.Extensions.Add("errors", errors);

        return await problemDetailsService.TryWriteAsync(context);
    }
}

public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Volontairement simple pour ne donner trop d'info au client car il s'agit d'une erreur interne
        return problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An error occurred while processing your request. Please try again"
            }
        });
    }
}
