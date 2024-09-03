using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace ecms.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult Match(
        this Result result,
        Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    public static IActionResult Match<TIn>(
        this Result<TIn> result,
        Func<TIn, IActionResult> onSuccess,
        Func<Result<TIn>, IActionResult> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }
}