namespace GlowGirlsBackend.Utility;

public class Result
{
    protected Result(bool succeeded, List<Error> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; }
    public List<Error> Errors { get; }

    public static Result Success()
    {
        return new Result(true, []);
    }

    private static Result Error(List<Error> errors)
    {
        return new Result(false, errors ?? throw new ArgumentNullException(nameof(errors)));
    }

    public static implicit operator Result(Error error)
    {
        return Error([error]);
    }

    public static implicit operator Result(List<Error> errors)
    {
        return Error(errors);
    }
}

public class Result<T> : Result
{
    private Result(T value)
        : base(true, [])
    {
        Value = value;
    }

    private Result(List<Error> errors)
        : base(false, errors) { }

    public T? Value { get; }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>([error]);
    }

    public static implicit operator Result<T>(List<Error> errors)
    {
        return new Result<T>(errors);
    }
}

public class Error(int code, string message)
{
    public int Code { get; } = code;
    public string Message { get; } = message;
}
