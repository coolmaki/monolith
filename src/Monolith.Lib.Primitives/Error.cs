namespace Monolith.Lib.Primitives;

public sealed record Error(ErrorType Type, string Code, string Description)
{
    public static Error Validation(string code, string description) => new(ErrorType.Validation, code, description);
    public static Error NotFound(string code, string description) => new(ErrorType.NotFound, code, description);
    public static Error Conflict(string code, string description) => new(ErrorType.Conflict, code, description);
    public static Error Unauthorized(string code, string description) => new(ErrorType.Unauthorized, code, description);
    public static Error Unexpected(string code, string description) => new(ErrorType.Unexpected, code, description);
}