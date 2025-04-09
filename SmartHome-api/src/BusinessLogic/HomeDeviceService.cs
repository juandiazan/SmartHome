using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;
public class HomeDeviceService(IHomeDeviceRepository homeDeviceRepository) : IHomeDeviceService
{
    public HomeDevice Create(CreateHomeDeviceArgs args)
    {
        if (!homeDeviceRepository.HomeExists(h => h.Id == args.HomeId))
        {
            throw new KeyNotFoundException("Home does not exist");
        }

        if (!homeDeviceRepository.DeviceExists(d => d.Id == args.DeviceId))
        {
            throw new KeyNotFoundException("Home does not exist");
        }

        var newHomeDevice = new HomeDevice
        {
            HomeId = args.HomeId,
            DeviceId = args.DeviceId,
            Alias = args.HomeDeviceAlias,
            ConnectionState = false
        };

        homeDeviceRepository.Add(newHomeDevice);

        return newHomeDevice;
    }

    public HomeDevice? GetHomeDeviceByHardwareId(Guid hardwareId)
    {
        if (HomeDeviceDoesNotExist(hardwareId))
        {
            throw new KeyNotFoundException("Home device does not exist");
        }

        return homeDeviceRepository.GetHomeDeviceByHardwareId(hardwareId)!;
    }

    public HomeDevice UpdateHomeDeviceAlias(UpdateHomeDeviceArgs args)
    {
        if (IsHomeDeviceIdInvalid(args))
        {
            throw new FormatException("Wrong device ID format");
        }

        if (IsAliasEmpty(args))
        {
            throw new ArgumentNullException(null, "New alias cannot be empty");
        }

        var homeDeviceToBeUpdated = homeDeviceRepository.GetHomeDeviceByHardwareId(Guid.Parse(args.HardwareId));

        if (homeDeviceToBeUpdated is null)
        {
            throw new KeyNotFoundException("Home device does not exist");
        }

        homeDeviceToBeUpdated.Alias = args.NewAlias;

        var updatedHomeDevice = homeDeviceRepository.UpdateHomeDevice(homeDeviceToBeUpdated);

        return updatedHomeDevice;
    }

    public bool UpdateHomeDeviceConnectionState(string hardwareId)
    {
        if (!Guid.TryParse(hardwareId, out var correctHardwareId))
        {
            throw new FormatException("Wrong device ID format");
        }

        var homeDeviceToBeUpdated = homeDeviceRepository.GetHomeDeviceByHardwareId(correctHardwareId);

        if (homeDeviceToBeUpdated is null)
        {
            throw new KeyNotFoundException("Home device does not exist");
        }

        homeDeviceToBeUpdated.ConnectionState = !homeDeviceToBeUpdated.ConnectionState;

        homeDeviceRepository.UpdateHomeDevice(homeDeviceToBeUpdated);

        return homeDeviceToBeUpdated.ConnectionState;
    }

    private bool HomeDeviceDoesNotExist(Guid hardwareId)
    {
        return homeDeviceRepository.GetHomeDeviceByHardwareId(hardwareId) is null;
    }

    private static bool IsAliasEmpty(UpdateHomeDeviceArgs args)
    {
        return string.IsNullOrEmpty(args.NewAlias);
    }

    private static bool IsHomeDeviceIdInvalid(UpdateHomeDeviceArgs args)
    {
        return !Guid.TryParse(args.HardwareId, out var _);
    }
}
