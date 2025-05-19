using FluentResults;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Utility.Result.ResultErrors;
using Utility.Result.ResultErrors.Enums;
using Status = Google.Rpc.Status;
using FieldViolation = Google.Rpc.BadRequest.Types.FieldViolation;

namespace BaboonCo.Utility.Grpc.Server;

public static class GrpcServerHelper
{
    public static RpcException CreateRpcException<T>(Result<T> result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Result is not failed.");

        var requestError = result.Errors.OfType<RequestError>().FirstOrDefault();
        if (requestError is null)
            throw new InvalidOperationException("Result MUST have one RequestError!");

        var errorCode = ToRpcStatusCode(requestError.Type);

        var errorDetail = new Status
        {
            Code = (int) errorCode,
            Message = requestError.Message,
        };

        if (errorCode is StatusCode.InvalidArgument)
        {
            errorDetail.Details.Add(Any.Pack(new BadRequest
            {
                FieldViolations = {ResultToFieldViolations(result)}
            }));
        }

        return errorDetail.ToRpcException();
    }

    private static StatusCode ToRpcStatusCode(RequestErrorType errorType)
    {
        return errorType switch
        {
            RequestErrorType.NotFound => StatusCode.NotFound,
            RequestErrorType.BadRequest => StatusCode.InvalidArgument,
            RequestErrorType.AlreadyExists => StatusCode.AlreadyExists,
            RequestErrorType.Internal => StatusCode.Internal,
            RequestErrorType.Unauthenticated => StatusCode.Unauthenticated,
            RequestErrorType.Unauthorized => StatusCode.PermissionDenied,
            _ => throw new ArgumentOutOfRangeException(nameof(errorType), errorType, "Unknown error type")
        };
    }

    private static RepeatedField<FieldViolation> ResultToFieldViolations<T>(Result<T> result)
    {
        return new RepeatedField<FieldViolation>
        {
            result.Errors
                .OfType<FieldError>()
                .Select(err => new FieldViolation
                {
                    Field = err.Field,
                    Description = err.Message
                })
        };
    }
}