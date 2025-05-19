using BaboonCo.Utility.Grpc.Client.Errors;
using FluentResults;
using Google.Rpc;
using Grpc.Core;
using Utility.Result.ResultErrors;
using Utility.Result.ResultErrors.Enums;

namespace BaboonCo.Utility.Grpc.Client;

public static class GrpcClientHelper
{
    private static Result ParseGrpcErrors(RpcException e)
    {
        var status = e.GetRpcStatus();
        if (status is null)
            throw new InvalidOperationException("[gRPC] Could not parse gRPC status because it is null.");

        var grpcResultError = new RequestError(e.Message, ToRequestErrorType(status.Code));
        var errors = new List<Error> {grpcResultError};

        if (status.Code == (int) StatusCode.InvalidArgument)
        {
            var detail = status.GetDetail<BadRequest>();
            var validationErrors = detail.FieldViolations
                .Select(fv => new ValidationError(fv.Field, fv.Description));
            errors.AddRange(validationErrors);
        }

        return Result.Fail(errors);
    }

    private static RequestErrorType ToRequestErrorType(int code)
    {
        var statusCode = (StatusCode) code;

        return statusCode switch
        {
            StatusCode.NotFound => RequestErrorType.NotFound,
            StatusCode.InvalidArgument => RequestErrorType.BadRequest,
            StatusCode.AlreadyExists => RequestErrorType.AlreadyExists,
            StatusCode.Internal => RequestErrorType.Internal,
            StatusCode.Unauthenticated => RequestErrorType.Unauthenticated,
            StatusCode.PermissionDenied => RequestErrorType.Unauthorized,
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, "Unknown error type")
        };
    }
}