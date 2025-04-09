using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;
using ImporterService;
using PaginationAndFilters;
using PaginationAndFilters.Models;

namespace BusinessLogic;

public sealed class DeviceService(
    IDeviceRepository sensorRepository,
    IModelValidatorAdapter modelValidator,
    IAssemblyLoadingService<IDeviceImporter> deviceImportersLoadingService,
    ICameraService cameraService,
    ISmartLampService smartLampService,
    ISessionService sessionService,
    ICompanyService companyService,
    IPathValidator pathValidator)
    : IDeviceService
{
    public Device Create(CreateDeviceArgs args, string authorization)
    {
        if (IsNotASensor(args, out DeviceType deviceType))
        {
            throw new ArgumentException("Device type must be Sensor or MovementSensor");
        }

        var user = sessionService.GetUserByToken(authorization);
        var company = companyService.GetCompanyByOwnerId(user.Id);

        if (!modelValidator.IsDeviceModelValid(company.DeviceModelValidatorId, args.DeviceModel))
        {
            throw new FormatException("Incorrect device model format");
        }

        var newSensor = new Device
        {
            CompanyId = company.Id,
            DeviceName = args.DeviceName,
            DeviceModel = args.DeviceModel,
            Description = args.Description,
            Photos = args.Photos,
            DeviceType = deviceType
        };

        sensorRepository.Add(newSensor);

        return newSensor;
    }

    public List<GetAllDevicesArgs> GetAll(DeviceFilterArgs args)
    {
        if (PaginationFilterService.CurrentPageOrPageSizeIsNegative(args.Offset, args.Limit))
        {
            throw new FormatException("Current page and page size cannot be negative or zero");
        }

        var devices = sensorRepository.GetAll(args);

        return devices.ConvertAll(device => new GetAllDevicesArgs(
            device.Id.ToString(),
            device.DeviceName,
            device.DeviceModel,
            device.Photos[0],
            device.CompanyItIsAssociatedTo.CompanyName,
            device.DeviceType.ToString()));
    }

    public List<string> GetAllDeviceTypes()
    {
        return [.. Enum.GetNames(typeof(DeviceType))];
    }

    public void ImportDevices(ImportDevicesArgs args, string token)
    {
        if (!Guid.TryParse(args.DeviceImporterImplementationId, out var deviceImporterId))
        {
            throw new FormatException("Invalid importer implementation");
        }

        if (!pathValidator.PathExists(args.Path))
        {
            throw new KeyNotFoundException("File not found");
        }

        deviceImportersLoadingService.LoadImplementations();

        var importer = deviceImportersLoadingService.GetImplementationById(deviceImporterId);

        var devices = importer.ImportDevices(args.Path);

        devices.ForEach(device =>
        {
            switch (device.DeviceType)
            {
                case nameof(DeviceType.Camera):
                    var cameraArgs = new CreateCameraArgs(
                    device.DeviceName,
                    device.DeviceModel,
                    "No Description",
                    SetPhotos(device.Photos),
                    DeviceType.Camera.ToString(),
                    false,
                    false,
                    device.HasMovementDetection ?? false,
                    device.HasPersonDetection ?? false);

                    cameraService.Create(cameraArgs, token);
                    break;

                case nameof(DeviceType.SmartLamp):
                    var lampArgs = new CreateSmartLampArgs(
                    device.DeviceName,
                    device.DeviceModel,
                    "No Description",
                    SetPhotos(device.Photos),
                    false,
                    device.DeviceType);

                    smartLampService.Create(lampArgs, token);
                    break;

                case nameof(DeviceType.Sensor) or nameof(DeviceType.MovementSensor):
                    var sensorArgs = new CreateDeviceArgs(
                    device.DeviceName,
                    device.DeviceModel,
                    "No Description",
                    SetPhotos(device.Photos),
                    device.DeviceType);

                    Create(sensorArgs, token);
                    break;

                default:
                    break;
            }
        });
    }

    private static bool IsNotASensor(CreateDeviceArgs args, out DeviceType deviceType)
    {
        return !Enum.TryParse<DeviceType>(args.DeviceType, out deviceType) ||
               (deviceType != DeviceType.Sensor && deviceType != DeviceType.MovementSensor);
    }

    private static List<string> SetPhotos(List<DevicePictureDTO> photosArgs)
    {
        var sortedPhotos = photosArgs
            .OrderByDescending(photo => photo.IsMain).ToList()
            .ConvertAll(p => p.Path).ToList();

        return sortedPhotos ?? [];
    }
}
