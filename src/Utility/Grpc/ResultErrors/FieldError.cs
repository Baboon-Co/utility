using FluentResults;

namespace Utility.Grpc.ResultErrors;

public class FieldError(string field, string code, string message) : Error(message)
{
    public string Field { get; } = field;
    public string Code { get; } = code;
}