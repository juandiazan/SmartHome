using Domain;
using PaginationAndFilters.Models;

namespace IDataAccess;

public interface IDeviceRepository : IAddRepository<Device>
{
    List<Device> GetAll(DeviceFilterArgs args);
}
