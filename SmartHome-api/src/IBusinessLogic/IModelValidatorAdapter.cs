namespace IBusinessLogic;
public interface IModelValidatorAdapter
{
    bool IsDeviceModelValid(Guid modelValidatorId, string deviceModel);
}
