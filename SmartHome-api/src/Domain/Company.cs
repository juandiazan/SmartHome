namespace Domain;
public sealed class Company
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string CompanyName { get; init; } = null!;
    public string Logotype { get; init; } = null!;
    public string Rut { get; init; } = null!;

    public CompanyOwner? CompanyOwner { get; init; }

    public List<Device> AssociatedDevices { get; init; } = [];

    public Guid DeviceModelValidatorId { get; init; }
}
