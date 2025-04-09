using DTOs;

namespace WebApi.Models.Requests;

public class CreateHomeRequest
{
    public string? MainStreet { get; init; }
    public int? DoorNumber { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public int? MaxAmountOfMembers { get; init; }
    public string? Alias { get; init; }

    public CreateHomeRequest(
        string? mainStreet,
        int? doorNumber,
        string? latitude,
        string? longitude,
        int? maxAmountOfMembers,
        string? alias)
    {
        MainStreet = mainStreet;
        DoorNumber = doorNumber;
        Latitude = latitude;
        Longitude = longitude;
        MaxAmountOfMembers = maxAmountOfMembers;
        Alias = alias;
    }

    public CreateHomeArgs ToArgs(string? ownerEmail)
    {
        var newArgs = new CreateHomeArgs(
            ownerEmail ?? string.Empty,
            MainStreet ?? string.Empty,
            DoorNumber ?? -1,
            Latitude ?? string.Empty,
            Longitude ?? string.Empty,
            MaxAmountOfMembers ?? -1,
            Alias ?? string.Empty);
        return newArgs;
    }
}
