using System;
using System.Collections.Generic;
using System.Linq;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FunctionHandler.Hndlers;

internal class BaseHandler
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors.FirstOrDefault());
    }

    protected IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return ValidationProblem(modelStateDictionary: modelStateDictionary);
    }

    public virtual OkObjectResult Ok([ActionResultObjectValue] object value)
        => new(value);

    public virtual ObjectResult Problem(
        string detail = null,
        string instance = null,
        int? statusCode = null,
        string title = null,
        string type = null)
    {
        var host = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
        var problemDetails = new ProblemDetails
        {
            Status = statusCode ?? 500,
            Title = title,
            Detail = detail,
            Type = type ?? $"{host}/errors"
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode,
            ContentTypes = { "application/problem+json" }
        };
    }

    public virtual ActionResult ValidationProblem(
        string detail = null,
        string instance = null,
        int? statusCode = null,
        string title = null,
        string type = null,
        [ActionResultObjectValue] ModelStateDictionary modelStateDictionary = null)
    {
        var validationProblem = new ValidationProblemDetails(modelStateDictionary)
        {
            Detail = detail,
            Instance = instance,
            Status = statusCode,
            Title = title,
            Type = type,
        };

        if (validationProblem is { Status: 400 })
        {
            return new BadRequestObjectResult(validationProblem);
        }

        return new ObjectResult(validationProblem)
        {
            StatusCode = validationProblem?.Status,
            ContentTypes = { "application/problem+json" }
        };
    }
}
