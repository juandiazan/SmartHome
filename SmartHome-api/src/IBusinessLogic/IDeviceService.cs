using Domain;
using DTOs;
using PaginationAndFilters.Models;

namespace IBusinessLogic;

public interface IDeviceService
{
    Device Create(CreateDeviceArgs args, string authorization);
    List<GetAllDevicesArgs> GetAll(DeviceFilterArgs args);
    List<string> GetAllDeviceTypes();
    void ImportDevices(ImportDevicesArgs args, string authorization);
}
