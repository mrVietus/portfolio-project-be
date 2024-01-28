using ErrorOr;

namespace Crawler.FunctionHandler.Errors;

public static class ApiErrors
{
    public static Error MissingUrlQueryParameter => Error.Validation(
            code: "Api.MissingQueryParameter",
            description: "Missing url query parameter.");

    public static Error WrongBody => Error.Validation(
            code: "Api.WrongBody",
            description: "Wrong body provided.");
}
