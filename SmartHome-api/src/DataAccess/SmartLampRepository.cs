using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SmartLampRepository : ISmartLampRepository
{
    private readonly SmartHomeDBContext _dbContext;
    private readonly DbSet<SmartLamp> _smartLamps;

    public SmartLampRepository(SmartHomeDBContext dbContext)
    {
        _dbContext = dbContext;
        _smartLamps = dbContext.Set<SmartLamp>();
    }

    public SmartLamp GetSmartLampByHardwareId(Guid smartLampId)
    {
        return _dbContext.SmartLamps.FirstOrDefault(c => c.Id == smartLampId)!;
    }

    public SmartLamp Update(SmartLamp smartLamp)
    {
        _dbContext.SmartLamps.Update(smartLamp);
        _dbContext.SaveChanges();
        return smartLamp;
    }

    public SmartLamp Add(SmartLamp args)
    {
        _smartLamps.Add(args);
        _dbContext.SaveChanges();

        return args;
    }
}
