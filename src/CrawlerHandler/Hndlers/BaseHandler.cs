using System.Net;
using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.Functions.Worker.Http;

namespace Crawler.FunctionHandler.Hndlers;

internal class BaseHandler
{
    protected async Task<HttpResponseData> ProblemAsync(HttpRequestData request, List<Error> errors, CancellationToken cancellationToken = default)
    {
        if (errors.Count is 0)
        {
            return await ProblemAsync(request, cancellationToken: cancellationToken);
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return await ValidationProblemAsync(request, errors, cancellationToken);
        }

        return await ProblemAsync(request, errors.FirstOrDefault(), cancellationToken);
    }

    protected async Task<HttpResponseData> ProblemAsync(HttpRequestData request, Error error, CancellationToken cancellationToken = default)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        return await ProblemAsync(
            request,
            statusCode: statusCode,
            title: error.Description,
            cancellationToken: cancellationToken
        );
    }

    private async Task<HttpResponseData> ValidationProblemAsync(HttpRequestData request, List<Error> errors, CancellationToken cancellationToken = default)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(
                error.Code,
                error.Description);
        }

        return await ValidationProblemAsync(
            request,
            modelStateDictionary: modelStateDictionary,
            cancellationToken: cancellationToken
        );
    }

    public virtual async Task<HttpResponseData> OkAsync(HttpRequestData request, object value, CancellationToken cancellationToken = default)
    {
        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(value, cancellationToken);

        return response;
    }

    public virtual async Task<HttpResponseData> CreatedAsync(HttpRequestData request, object value, CancellationToken cancellationToken = default)
    {
        var response = request.CreateResponse();
        await response.WriteAsJsonAsync(value, cancellationToken);
        response.StatusCode = HttpStatusCode.Created;

        return response;
    }

    public virtual async Task<HttpResponseData> ProblemAsync(
        HttpRequestData request,
        string detail = null,
        string instance = null,
        HttpStatusCode? statusCode = null,
        string title = null,
        string type = null,
        CancellationToken cancellationToken = default)
    {
        var host = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
        var problemDetails = new ProblemDetails
        {
            Status = ((int?)statusCode) ?? 500,
            Title = title,
            Detail = detail,
            Type = type ?? $"{host}/errors"
        };

        var response = request.CreateResponse();
        await response.WriteAsJsonAsync(problemDetails, cancellationToken);
        response.StatusCode = statusCode ?? HttpStatusCode.InternalServerError;

        return response;
    }

    public virtual async Task<HttpResponseData> ValidationProblemAsync(
        HttpRequestData request,
        ModelStateDictionary modelStateDictionary,
        string detail = null,
        string instance = null,
        HttpStatusCode? statusCode = null,
        string title = null,
        string type = null,
        CancellationToken cancellationToken = default)
    {
        var validationProblem = new ValidationProblemDetails(modelStateDictionary)
        {
            Detail = detail,
            Instance = instance,
            Status = (int?)statusCode,
            Title = title,
            Type = type,
        };

        if (validationProblem is { Status: 400 })
        {
            var badRequestResponse = request.CreateResponse();
            await badRequestResponse.WriteAsJsonAsync(validationProblem, cancellationToken);
            badRequestResponse.StatusCode = HttpStatusCode.BadRequest;

            return badRequestResponse;
        }

        var response = request.CreateResponse();
        await response.WriteAsJsonAsync(validationProblem, cancellationToken);
        response.StatusCode = statusCode ?? HttpStatusCode.InternalServerError;

        return response;
    }

    internal static T TryDeserializeRequestBody<T>(Stream requestBody)
    {
        try
        {
            using StreamReader reader = new(requestBody);
            string requestBodyText = reader.ReadToEnd();

            return JsonSerializer.Deserialize<T>(requestBodyText);
        }
        catch (Exception)
        {
            return default;
        }
    }
}
