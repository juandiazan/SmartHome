using Domain;
using DTOs;
using IBusinessLogic;
using IDataAccess;
namespace BusinessLogic;

public class SmartLampService(
    ISmartLampRepository smartLampRepository,
    ISessionService sessionService,
    ICompanyService companyService,
    IModelValidatorAdapter modelValidator) : ISmartLampService
{
    public SmartLamp Create(CreateSmartLampArgs args, string token)
    {
        if (DeviceTypeIsNotSmartLamp(args))
        {
            throw new ArgumentException("Device type must be SmartLamp");
        }

        var user = sessionService.GetUserByToken(token);
        var company = companyService.GetCompanyByOwnerId(user.Id);

        if (!modelValidator.IsDeviceModelValid(company.DeviceModelValidatorId, args.DeviceModel))
        {
            throw new FormatException("Incorrect device model format");
        }

        var newSmartLamp = new SmartLamp
        {
            DeviceName = args.DeviceName,
            DeviceModel = args.DeviceModel,
            Description = args.Description,
            Photos = args.Photos,
            DeviceType = DeviceType.SmartLamp,
            CompanyId = company.Id,
            IsTurnedOn = args.IsTurnedOn
        };

        smartLampRepository.Add(newSmartLamp);

        return newSmartLamp;
    }

    public bool ChangeState(Guid smartLampId)
    {
        var smartLamp = smartLampRepository.GetSmartLampByHardwareId(smartLampId);
        smartLamp.IsTurnedOn = !smartLamp.IsTurnedOn;
        smartLampRepository.Update(smartLamp);

        return smartLamp.IsTurnedOn;
    }

    private bool DeviceTypeIsNotSmartLamp(CreateSmartLampArgs args)
    {
        return args.DeviceType != DeviceType.SmartLamp.ToString();
    }
}
