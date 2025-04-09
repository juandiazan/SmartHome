using Domain;

namespace PaginationAndFilters.Models;
public sealed record class DeviceFilterArgs : PaginationArgs
{
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? CompanyName { get; set; }
    public DeviceType? DeviceType { get; set; }

    public DeviceFilterArgs()
        : base(null, null)
    {
    }

    public DeviceFilterArgs(
        int? offset = 1,
        int? limit = 10,
        string? deviceName = null,
        string? model = null,
        string? companyName = null,
        DeviceType? deviceType = null)
        : base(offset, limit)
    {
        DeviceName = deviceName;
        Model = model;
        CompanyName = companyName;
        DeviceType = deviceType;
    }
}
