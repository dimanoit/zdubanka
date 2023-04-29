namespace Domain.Models;

public record Result<TResult>
{
    private Result(TResult value, RestErrorDetails? error = null)
    {
        Value = value;
        Error = error;
    }

    public TResult Value { get; }
    public RestErrorDetails? Error { get; }

    public bool IsSuccess => Error == null;

    public static Result<TResult?> Success(TResult? value = default)
    {
        return new Result<TResult?>(value, null);
    }

    public static Result<TResult?> Failure(RestErrorDetails error)
    {
        return new Result<TResult?>(default, error);
    }
}

public record Result
{
    private Result(dynamic value, RestErrorDetails? error)
    {
        Value = value;
        Error = error;
    }

    public dynamic Value { get; }
    public RestErrorDetails? Error { get; }

    public bool IsSuccess => Error == null;

    public static Result Success(dynamic value = default)
    {
        return new Result(value, null);
    }

    public static Result Failure(RestErrorDetails error)
    {
        return new Result(default, error);
    }
}