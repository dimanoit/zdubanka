using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ApplicationUnitTests.Helpers;

public static class DbConfigurationHelper
{
    public static void HasJsonConversion<T, TResponse>(
        this ModelBuilder modelBuilder,
        Expression<Func<T, TResponse>> propertyExpression) where T : class
    {
        modelBuilder.Entity<T>()
            .Property(propertyExpression)
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v =>
                    JsonSerializer.Deserialize<TResponse>(v, new JsonSerializerOptions())!
            );
    }
}