﻿using GlobalErrorApp.Exceptions;
using Serilog;
using System.Net;
using System.Text.Json;

namespace GlobalErrorApp.Configurations
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            var message = string.Empty;
            var exceptionType = ex.GetType();
            if(exceptionType == typeof(BadRequestException))
            {
                message = ex.Message;
                status = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace; 
            }
            else if(exceptionType == typeof(NotFoundException))
            {
                message = ex.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
            }
            else if(exceptionType == typeof(Exceptions.NotImplementedException))
            {
                message = ex.Message;
                status = HttpStatusCode.NotImplemented;
                stackTrace = ex.StackTrace; 
            }
            else if(exceptionType == typeof(Exceptions.KeyNotFoundException))
            {
                message = ex.Message;
                status = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
            }
            else if(exceptionType == typeof(Exceptions.UnauthorizedAccessException))
            {
                message = ex.Message;
                status = HttpStatusCode.Unauthorized;
                stackTrace = ex.StackTrace;
            }
            else
            {
                message = ex.Message;
                status = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
            }
            var exceptionResult = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            _logger.LogError(exceptionResult, ex);
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
