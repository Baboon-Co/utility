using BaboonCo.Utility.Result.ResultErrors.Enums;
using FluentResults;

namespace BaboonCo.Utility.Result.ResultErrors;

public class RequestError(string reason, RequestErrorType type) : Error(reason)
{
    public RequestErrorType Type { get; } = type;
}