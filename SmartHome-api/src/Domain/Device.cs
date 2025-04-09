namespace Domain;
public class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string DeviceName { get; init; } = null!;
    public string DeviceModel { get; init; } = null!;
    public string Description { get; init; } = null!;
    public List<string> Photos { get; set; } = null!;
    public DeviceType DeviceType { get; set; }

    public Guid CompanyId { get; set; }
    public Company CompanyItIsAssociatedTo { get; set; } = null!;

    public ICollection<HomeDevice> HomeDevices { get; init; } = [];
}
