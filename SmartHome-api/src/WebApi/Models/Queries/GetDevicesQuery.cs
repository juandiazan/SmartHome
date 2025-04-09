using Domain;
using PaginationAndFilters.Models;

namespace WebApi.Models.Queries;

public sealed class GetDevicesQuery
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
    public string? DeviceName { get; set; }
    public string? Model { get; set; }
    public string? CompanyName { get; set; }
    public DeviceType? DeviceType { get; set; }

    public GetDevicesQuery()
    {
    }

    public GetDevicesQuery(
        int? offset,
        int? limit,
        string? deviceName,
        string? model,
        string? companyName,
        DeviceType? deviceType)
    {
        Offset = offset;
        Limit = limit;
        DeviceName = deviceName;
        Model = model;
        CompanyName = companyName;
        DeviceType = deviceType;
    }

    public DeviceFilterArgs ToArgs()
    {
        return new DeviceFilterArgs(Offset, Limit, DeviceName, Model, CompanyName, DeviceType);
    }
}
