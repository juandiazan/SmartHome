using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace DataAccess;
public class DeviceRepository(SmartHomeDBContext context) : IDeviceRepository
{
    private readonly DbSet<Device> _devices = context.Set<Device>();

    public Device Add(Device newDevice)
    {
        _devices.Add(newDevice);

        context.SaveChanges();

        return newDevice;
    }

    public List<Device> GetAll(DeviceFilterArgs args)
    {
        var devices = _devices.Include(d => d.CompanyItIsAssociatedTo);

        return PaginationFilterService.FilterAndPaginateDevices(devices, args);
    }
}
