namespace Integration.Fixtures;

public static class CoordinatesFixture
{
    private static readonly Random random = new();

    public static double GenerateLatitude(double minLatitude = -90.0, double maxLatitude = 90.0)
    {
        return random.NextDouble() * (maxLatitude - minLatitude) + minLatitude;
    }

    public static double GenerateLongitude(double minLongitude = -180.0, double maxLongitude = 180.0)
    {
        return random.NextDouble() * (maxLongitude - minLongitude) + minLongitude;
    }
}