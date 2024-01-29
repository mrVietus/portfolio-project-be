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

    public static Error IdIsNotCorrectValue => Error.Validation(
            code: "Api.WrongId",
            description: "Id is not a correct value.");

    public static Error WrongDataProvided => Error.Validation(
            code: "Api.WrongDataProvided",
            description: "Wrong data has been provided to the api.");
}
