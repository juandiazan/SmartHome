using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class CameraRepository : IAddRepository<Camera>
{
    private readonly SmartHomeDBContext _dbContext;
    private readonly DbSet<Camera> _cameras;

    public CameraRepository(SmartHomeDBContext dbContext)
    {
        _dbContext = dbContext;
        _cameras = dbContext.Set<Camera>();
    }

    public Camera Add(Camera args)
    {
        _cameras.Add(args);
        _dbContext.SaveChanges();

        return args;
    }
}
