namespace Application.Helpers;

public static class GeoHelper
{
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
    {
        const double earthRadiusKm = 6371;

        var lat1Rad = ToRadians(latitude1);
        var lat2Rad = ToRadians(latitude2);
        var deltaLat = lat2Rad - lat1Rad;

        var lon1Rad = ToRadians(longitude1);
        var lon2Rad = ToRadians(longitude2);
        var deltaLon = lon2Rad - lon1Rad;

        var a =
            Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
            Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
            Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = earthRadiusKm * c;

        return distance;
    }
}