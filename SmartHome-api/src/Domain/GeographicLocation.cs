namespace Domain;
public class GeographicLocation(
    string latitude,
    string longitude)
{
    public string Latitude { get; } = latitude;
    public string Longitude { get; } = longitude;
}
