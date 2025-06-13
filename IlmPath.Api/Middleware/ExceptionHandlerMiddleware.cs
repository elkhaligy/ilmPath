using IlmPath.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace IlmPath.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
            ProblemDetails problemDetails = new();

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    problemDetails = new ValidationProblemDetails(validationException.Errors)
                    {
                        Title = "Validation Error",
                        Status = (int)statusCode,
                        Detail = "One or more validation errors occurred."
                    };
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    problemDetails = new ProblemDetails
                    {
                        Title = "Resource Not Found",
                        Status = (int)statusCode,
                        Detail = notFoundException.Message
                    };
                    break;

                // You can add more custom exception cases here
                // case BadRequestException badRequestException:
                //     ...

                default:
                    problemDetails = new ProblemDetails
                    {
                        Title = "An unexpected error occurred",
                        Status = (int)statusCode,
                        Detail = "An internal server error has occurred. Please try again later."
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/problem+json";

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}