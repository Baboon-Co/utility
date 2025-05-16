using FluentResults;
using Utility.Result.ResultErrors.Enums;

namespace Utility.Result.ResultErrors;

public class RequestError(string reason, RequestErrorType type) : Error(reason)
{
    public RequestErrorType Type { get; } = type;
}