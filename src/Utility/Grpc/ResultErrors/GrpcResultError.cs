using FluentResults;
using Utility.Grpc.ResultErrors.Enums;

namespace Utility.Grpc.ResultErrors;

public class GrpcResultError(ResponseErrorType errorType, string message = "Error occured.") : Error(message)
{
    public ResponseErrorType ErrorType { get; } = errorType;
}