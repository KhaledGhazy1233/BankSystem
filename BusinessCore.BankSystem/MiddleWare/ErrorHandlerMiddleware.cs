using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Text.Json;

namespace BusinessCore.BankSystem.MiddleWare
{
    /// <summary>
    /// Global error handling middleware
    /// Catches all unhandled exceptions and converts them to standardized Response objects
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger = null)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // Log the error with full details
            LogError(error, context);

            // Create appropriate response based on exception type
            Response<string> responseModel = error switch
            {
                // ✅ Unauthorized Access
                UnauthorizedAccessException => Response<string>.Unauthorized(
                    error.Message ?? "Unauthorized access"
                ),

                // ✅ Validation Errors (FluentValidation)
                ValidationException validationEx => Response<string>.ValidationError(
                    validationEx.Errors?.Select(e => e.ErrorMessage).ToList()
                    ?? new List<string> { validationEx.Message }
                ),

                // ✅ Not Found
                KeyNotFoundException => Response<string>.NotFound(
                    error.Message ?? "Resource not found"
                ),

                // ✅ Database Update Errors
                DbUpdateException dbEx => Response<string>.BadRequest(
                    GetDatabaseErrorMessage(dbEx)
                ),



                // ✅ ArgumentException / ArgumentNullException
                ArgumentException or ArgumentNullException => Response<string>.BadRequest(
                    error.Message ?? "Invalid argument provided"
                ),

                // ✅ InvalidOperationException
                InvalidOperationException => Response<string>.BadRequest(
                    error.Message ?? "Invalid operation"
                ),

                // ✅ TimeoutException
                TimeoutException => Response<string>.Failure(
                    "The operation timed out. Please try again.",
                    HttpStatusCode.RequestTimeout
                ),

                // ✅ Custom ApiException (if you have one)
                _ when error.GetType().Name == "ApiException" => Response<string>.BadRequest(
                    GetFullErrorMessage(error)
                ),

                // ❌ Unhandled / Internal Server Error
                _ => Response<string>.InternalServerError(
                    GetSafeErrorMessage(error)
                )
            };

            // Set HTTP status code
            response.StatusCode = (int)responseModel.StatusCode;

            // Serialize and write response
            var result = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await response.WriteAsync(result);
        }

        /// <summary>
        /// Logs error with context information
        /// </summary>
        private void LogError(Exception error, HttpContext context)
        {
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var userId = context.User?.Identity?.Name ?? "Anonymous";

            // Using Serilog
            Log.Error(error,
                "Unhandled exception occurred. Path: {RequestPath}, Method: {RequestMethod}, User: {UserId}",
                requestPath, requestMethod, userId);

            // Alternative: Using ILogger (if injected)
            _logger?.LogError(error,
                "Unhandled exception occurred. Path: {RequestPath}, Method: {RequestMethod}, User: {UserId}",
                requestPath, requestMethod, userId);
        }

        /// <summary>
        /// Extracts meaningful message from database exceptions
        /// </summary>
        private string GetDatabaseErrorMessage(DbUpdateException dbEx)
        {
            // Check for common database errors
            var innerException = dbEx.InnerException?.Message ?? "";

            if (innerException.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                return "A record with this value already exists.";
            }

            if (innerException.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
            {
                return "Cannot perform this operation due to related records.";
            }

            if (innerException.Contains("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                return "Cannot delete this record because it is referenced by other records.";
            }

            // Fallback to general message
            return "A database error occurred. Please try again.";
        }

        /// <summary>
        /// Gets full error message including inner exceptions
        /// </summary>
        private string GetFullErrorMessage(Exception error)
        {
            var message = error.Message;

            if (error.InnerException != null)
            {
                message += $"\n{error.InnerException.Message}";
            }

            return message;
        }

        /// <summary>
        /// Gets safe error message (hides sensitive details in production)
        /// </summary>
        private string GetSafeErrorMessage(Exception error)
        {
            // In production, you might want to hide error details
#if DEBUG
            return GetFullErrorMessage(error);
#else
                // In production, return generic message and log the real error
                return "An unexpected error occurred. Please try again later.";
#endif
        }
    }
}

