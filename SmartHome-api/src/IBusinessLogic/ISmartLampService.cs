using Domain;
using DTOs;

namespace IBusinessLogic;

public interface ISmartLampService
{
    SmartLamp Create(CreateSmartLampArgs args, string token);
    bool ChangeState(Guid smartLampId);
}
