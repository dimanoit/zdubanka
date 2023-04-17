namespace Domain.Response.Common;

public abstract record PageResponseBase<T>
{
    public IEnumerable<T> Data { get; set; }
    public int TotalCount { get; set; }
}