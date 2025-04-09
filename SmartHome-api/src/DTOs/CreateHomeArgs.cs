namespace DTOs;
public class CreateHomeArgs
{
    private const int MinimumMainStreetNumberPossible = 0;
    private const int MinAmountOfMaxMembersPossible = 1;

    public string OwnerEmail { get; init; }
    public string MainStreet { get; init; }
    public int DoorNumber { get; init; }
    public string Latitude { get; init; }
    public string Longitude { get; init; }
    public int MaxAmountOfMembers { get; init; }
    public string Alias { get; init; } = string.Empty;

    public CreateHomeArgs(
        string ownerEmail,
        string mainStreet,
        int doorNumber,
        string latitude,
        string longitude,
        int maxAmountOfMembers,
        string alias)
    {
        if (HasNullOrEmptyMainStreet(mainStreet))
        {
            throw new ArgumentNullException(null, "Main street cannot be null or empty");
        }

        if (HasNullOrEmptyLatitude(latitude))
        {
            throw new ArgumentNullException(null, "Latitude cannot be null or empty");
        }

        if (HasNullOrEmptyLongitude(longitude))
        {
            throw new ArgumentNullException(null, "Longitude cannot be null or empty");
        }

        if (HasNullOrEmptyAlias(alias))
        {
            throw new ArgumentNullException(null, "Alias cannot be null or empty");
        }

        if (HasInvalidDoorNumber(doorNumber))
        {
            throw new FormatException("Door number cannot be less than zero");
        }

        if (HasInvalidMaxAmountOfMembers(maxAmountOfMembers))
        {
            throw new FormatException("Maximum amount of members cannot be less than zero");
        }

        OwnerEmail = ownerEmail;
        MainStreet = mainStreet;
        DoorNumber = doorNumber;
        Latitude = latitude;
        Longitude = longitude;
        MaxAmountOfMembers = maxAmountOfMembers;
        Alias = alias;
    }

    private static bool HasNullOrEmptyMainStreet(string mainStreet)
    {
        return string.IsNullOrEmpty(mainStreet);
    }

    private static bool HasNullOrEmptyLatitude(string latitude)
    {
        return string.IsNullOrEmpty(latitude);
    }

    private static bool HasNullOrEmptyLongitude(string longitude)
    {
        return string.IsNullOrEmpty(longitude);
    }

    private static bool HasNullOrEmptyAlias(string alias)
    {
        return string.IsNullOrEmpty(alias);
    }

    private static bool HasInvalidDoorNumber(int doorNumber)
    {
        return doorNumber <= MinimumMainStreetNumberPossible;
    }

    private static bool HasInvalidMaxAmountOfMembers(int maxAmountOfMembers)
    {
        return maxAmountOfMembers <= MinAmountOfMaxMembersPossible;
    }
}
