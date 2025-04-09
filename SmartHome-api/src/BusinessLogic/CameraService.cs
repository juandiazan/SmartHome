using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;

namespace BusinessLogic;

public sealed class CameraService(
    IAddRepository<Camera> cameraRepository,
    ISessionService sessionService,
    ICompanyService companyService,
    IModelValidatorAdapter modelValidator) : ICameraService
{
    public Camera Create(CreateCameraArgs args, string auth)
    {
        var user = sessionService.GetUserByToken(auth);
        var company = companyService.GetCompanyByOwnerId(user.Id);

        if (ValidateDeviceType(args, out DeviceType deviceType))
        {
            throw new ArgumentException("Device type must be camera");
        }

        if (!modelValidator.IsDeviceModelValid(company.DeviceModelValidatorId, args.DeviceModel))
        {
            throw new FormatException("Incorrect device model format");
        }

        var newCamera = new Camera
        {
            CompanyId = company.Id,
            DeviceName = args.DeviceName,
            DeviceModel = args.DeviceModel,
            Description = args.Description,
            Photos = args.Photos,
            DeviceType = deviceType,
            CanBeUsedIndoors = args.CanBeUsedIndoors,
            CanBeUsedOutdoors = args.CanBeUsedOutdoors,
            HasMovementDetectionSupport = args.HasMovementDetectionSupport,
            HasPersonDetectionSupport = args.HasPersonDetectionSupport
        };

        cameraRepository.Add(newCamera);

        return newCamera;
    }

    private static bool ValidateDeviceType(CreateCameraArgs args, out DeviceType deviceType)
    {
        return !Enum.TryParse<DeviceType>(args.DeviceType, out deviceType) || deviceType != DeviceType.Camera;
    }
}
