namespace Monolith.Lib.Primitives;

public readonly struct Result<T>
{
    // ------------------------------------------------------------
    // Constructors & Factories
    // ------------------------------------------------------------

    private Result(T value)
    {
        _value = value;
        _error = null;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        _value = default;
        _error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Failure(error);

    // ------------------------------------------------------------
    // Backing Fields
    // ------------------------------------------------------------

    private readonly T? _value;
    private readonly Error? _error;

    // ------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value => IsSuccess ? _value! : throw new InvalidOperationException();

    public Error Error => IsFailure ? _error! : throw new InvalidOperationException();

    // ------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
    {
        return IsSuccess
            ? onSuccess(_value!)
            : onFailure(_error!);
    }

    public Result<TOut> Map<TOut>(Func<T, TOut> map)
    {
        return IsSuccess
            ? map(_value!)
            : _error!;
    }

    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> bind)
    {
        return IsSuccess
            ? bind(_value!)
            : _error!;
    }

    public Result<TNext> And<TNext>(Result<TNext> next)
    {
        return IsSuccess
            ? next
            : _error!;
    }

    public Result<TNext> AndThen<TNext>(Func<T, Result<TNext>> next)
    {
        return IsSuccess
            ? next(_value!)
            : _error!;
    }

    public Result<T> Or(Result<T> next)
    {
        return IsSuccess
            ? this
            : next;
    }

    public Result<T> OrElse(Func<Error, Result<T>> next)
    {
        return IsSuccess
            ? this
            : next(_error!);
    }
}