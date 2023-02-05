using ADT.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ADT.Api.Extensions;

public static class OperationResultExtensions
{
    internal static async Task<IResult> ToActionResult<T>(this Task<OperationResult<T>> operation)
    {
        var result = await operation;

        return result.Status switch
        {
            OperationResultStatus.Success => Results.Ok(result.Data),
            OperationResultStatus.BadRequest => BadRequest("Bad request", result.ErrorMessage),
            OperationResultStatus.Forbidden => Results.Forbid(),
            _ => Results.Problem(new ProblemDetails()
            {
                Title = "Internal server error",
                Status = StatusCodes.Status500InternalServerError
            }),
        };
    }

    internal static async Task<IResult> ToActionResult(this Task<OperationResultStatus> operation)
    {
        var status = await operation;

        return status switch
        {
            OperationResultStatus.Success => Results.NoContent(),
            OperationResultStatus.BadRequest => BadRequest("Bad request", null),
            OperationResultStatus.Forbidden => Results.Forbid(),
            _ => Results.Problem(new ProblemDetails()
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error"
            }),
        };
    }

    private static IResult BadRequest(string title, string? detail)
    {
        return Results.BadRequest(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = title,
            Detail = detail
        });
    }
}