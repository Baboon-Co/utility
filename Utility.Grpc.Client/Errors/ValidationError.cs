using FluentResults;

namespace BaboonCo.Utility.Grpc.Client.Errors;

public class ValidationError(string field, string message) : Error(message)
{
    public string Field { get; } = field;
}