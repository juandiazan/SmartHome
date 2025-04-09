using System.Linq.Expressions;
using Domain;

namespace IDataAccess;
public interface IHomeDeviceRepository : IAddRepository<HomeDevice>
{
    bool HomeExists(Expression<Func<Home, bool>> predicate);
    bool DeviceExists(Expression<Func<Device, bool>> predicate);
    HomeDevice? GetHomeDeviceByHardwareId(Guid hardwareId);

    HomeDevice UpdateHomeDevice(HomeDevice deviceToBeUpdated);
}
