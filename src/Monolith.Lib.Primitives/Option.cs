namespace Monolith.Lib.Primitives;

public readonly struct Option<T>
{
    // ------------------------------------------------------------
    // Constructors & Factories
    // ------------------------------------------------------------

    private Option(T? value, bool isSome)
    {
        _value = value;
        IsSome = isSome;
    }

    public static Option<T> Some(T value) => new(value, isSome: true);

    public static Option<T> None() => new(default, isSome: false);

    public static implicit operator Option<T>(T value) => Some(value);

    // ------------------------------------------------------------
    // Backing Fields
    // ------------------------------------------------------------

    private readonly T? _value;

    // ------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------

    public bool IsSome { get; }

    public bool IsNone => !IsSome;

    public T Value => IsSome ? _value! : throw new InvalidOperationException();

    // ------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------

    public TOut Match<TOut>(Func<T, TOut> onSome, Func<TOut> onNone)
    {
        return IsSome
            ? onSome(_value!)
            : onNone();
    }

    public Option<TOut> Map<TOut>(Func<T, TOut> map)
    {
        return IsSome
            ? map(_value!)
            : Option<TOut>.None();
    }

    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind)
    {
        return IsSome
            ? bind(_value!)
            : Option<TOut>.None();
    }

    public Option<TNext> And<TNext>(Option<TNext> next)
    {
        return IsSome
            ? next
            : Option<TNext>.None();
    }

    public Option<TNext> AndThen<TNext>(Func<T, Option<TNext>> next)
    {
        return IsSome
            ? next(_value!)
            : Option<TNext>.None();
    }

    public Option<T> Or(Option<T> next)
    {
        return IsSome
            ? this
            : next;
    }

    public Option<T> OrElse(Func<Option<T>> next)
    {
        return IsSome
            ? this
            : next();
    }
}