using System.Linq.Expressions;
using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;
public class HomeDeviceRepository : IHomeDeviceRepository
{
    private readonly SmartHomeDBContext _context;
    private readonly DbSet<HomeDevice> _homeDevices;
    public HomeDeviceRepository(SmartHomeDBContext context)
    {
        _context = context;
        _homeDevices = context.Set<HomeDevice>();
    }

    public HomeDevice Add(HomeDevice newHomeDevice)
    {
        _homeDevices.Add(newHomeDevice);

        _context.SaveChanges();

        return newHomeDevice;
    }

    public bool DeviceExists(Expression<Func<Device, bool>> predicate)
    {
        return _context.Set<Device>().Any(predicate);
    }

    public bool HomeExists(Expression<Func<Home, bool>> predicate)
    {
        return _context.Set<Home>().Any(predicate);
    }

    public HomeDevice? GetHomeDeviceByHardwareId(Guid hardwareId)
    {
        return _homeDevices.FirstOrDefault(hd => hd.HardwareId == hardwareId);
    }

    public HomeDevice UpdateHomeDevice(HomeDevice deviceToUpdate)
    {
        _homeDevices.Update(deviceToUpdate);
        _context.SaveChanges();
        return deviceToUpdate;
    }
}
