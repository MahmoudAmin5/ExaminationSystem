using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExaminationSystem.Api.Middlewares.GlobalExceptionMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (OperationCanceledException) when (httpContext.RequestAborted.IsCancellationRequested)
            {

                _logger.LogWarning(
                    "Request was cancelled. Path: {Path}",
                    httpContext.Request.Path);

                httpContext.Response.StatusCode = 499;
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed for request. Path: {Path}", httpContext.Request.Path);
                await HandleValidationExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity; 
           
            var validationErrors = exception.Errors
                .Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                }).ToList();

            var response = new
            {
                success = false,
                data = (object?)null,
                error = new
                {
                    code = "ValidationError",
                    message = "One or more validation errors occurred.",
                    details = validationErrors 
                },
                meta = new { traceId = context.TraceIdentifier }
            };

            await context.Response.WriteAsJsonAsync(response);
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new
            {
                success = false,
                data = (object?)null,
                error = new
                {
                    code = "InternalServerError",
                    message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred while processing your request.",
                    details = _env.IsDevelopment() ? exception.StackTrace : null
                },
                meta = new { traceId = context.TraceIdentifier }
            };

            await context.Response.WriteAsJsonAsync(response);


        }
    } }






        
        

      

