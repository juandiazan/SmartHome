namespace WebApi.Models.Responses;

public class CreateHomeResponse
{
    public string OwnerEmail { get; set; }
    public string MainStreet { get; init; }
    public int DoorNumber { get; init; }
    public string Latitude { get; init; }
    public string Longitude { get; init; }
    public int MaxAmountOfMembers { get; init; }
    public string Alias { get; init; }

    public CreateHomeResponse(
        string ownerEmail,
        string mainStreet,
        int doorNumber,
        string latitude,
        string longitude,
        int maxAmountOfMembers,
        string alias)
    {
        OwnerEmail = ownerEmail;
        MainStreet = mainStreet;
        DoorNumber = doorNumber;
        Latitude = latitude;
        Longitude = longitude;
        MaxAmountOfMembers = maxAmountOfMembers;
        Alias = alias;
    }
}
