using Domain;

namespace IDataAccess;

public interface ISmartLampRepository : IAddRepository<SmartLamp>
{
    SmartLamp GetSmartLampByHardwareId(Guid smartLampId);
    SmartLamp Update(SmartLamp smartLamp);
}
