using Domain;
using DTOs;

namespace IBusinessLogic;

public interface IHomeDeviceService
{
    HomeDevice Create(CreateHomeDeviceArgs args);
    HomeDevice? GetHomeDeviceByHardwareId(Guid hardwareId);
    HomeDevice UpdateHomeDeviceAlias(UpdateHomeDeviceArgs args);
    bool UpdateHomeDeviceConnectionState(string hardwareId);
}
