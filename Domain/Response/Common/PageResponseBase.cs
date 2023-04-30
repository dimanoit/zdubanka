namespace Domain.Response.Common;

public abstract record PageResponseBase<T>
{
    public IEnumerable<T> Data { get; set; } = Array.Empty<T>();
    public int TotalCount { get; set; }
}