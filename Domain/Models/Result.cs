namespace Domain.Models;

public record Result<TResult>
{
    private Result(TResult value, Exception? exception = null)
    {
        Value = value;
        Exception = exception;
    }

    public TResult Value { get; }
    public Exception? Exception { get; }

    public bool IsSuccess => Exception == null;

    public static Result<TResult> Success(TResult value)
    {
        return new Result<TResult>(value, null);
    }

    public static Result<TResult?> Failure(Exception exception)
    {
        return new Result<TResult?>(default, exception);
    }
}