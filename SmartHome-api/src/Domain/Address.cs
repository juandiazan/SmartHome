namespace Domain;
public class Address(
    string mainStreet,
    int doorNumber)
{
    public string MainStreet { get; } = mainStreet;
    public int DoorNumber { get; } = doorNumber;
}
