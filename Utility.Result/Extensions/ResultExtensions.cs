using BaboonCo.Utility.Result.ResultErrors;

namespace BaboonCo.Utility.Result.Extensions;

public static class ResultExtensions
{
    public static RequestError GetRequestError<T>(this FluentResults.Result<T> result)
    {
        var grpcError = result.Errors.OfType<RequestError>().FirstOrDefault();
        if (grpcError is null)
            throw new NullReferenceException("Required gRPC result error is null.");

        return grpcError;
    }
}