using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AreaAccountApi.Filters;

public class HttpGlobalExceptionFilter : IExceptionFilter
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<HttpGlobalExceptionFilter> _logger;

    public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(new EventId(context.Exception.HResult),
            context.Exception,
            context.Exception.Message);

        var jsonErrorResponse = new ErrorResponse
        {
            Messages = new[] { context.Exception.Message }
        };

        if (_env.IsDevelopment())
        {
            jsonErrorResponse.Exception = context.Exception.ToString();
        }

        context.Result = new ObjectResult(jsonErrorResponse)
        {
            StatusCode = context.Exception is ArgumentException
                ? StatusCodes.Status400BadRequest
                : StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}

public class ErrorResponse
{
    public IEnumerable<string> Messages { get; set; }

    [DataMember(EmitDefaultValue = false)] public string Exception { get; set; }
}