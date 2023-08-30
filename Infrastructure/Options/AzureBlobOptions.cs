namespace Infrastructure.Options;

public class AzureBlobOptions
{
    public string ConnectionString { get; init; } = null!;
    public string ContainerName { get; init; } = null!;
}