﻿using ErrorOr;

namespace FunctionHandler.Errors;

public static class ApiErrors
{
    public static Error MissingUrlQueryParameter => Error.Validation(
            code: "Api.MissingQueryParameter",
            description: "Missing url query parameter.");
}
