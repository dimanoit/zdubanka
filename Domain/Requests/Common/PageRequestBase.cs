namespace Domain.Requests.Common;

public abstract record PageRequestBase
{
    public int Skip { get; init; } = 0;

    public int Take { get; init; } = 20;
}