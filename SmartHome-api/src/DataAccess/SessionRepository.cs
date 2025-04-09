using DataAccess.DBContext;
using Domain;
using IDataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;
public class SessionRepository : ISessionRepository, IUserHasSessionActive
{
    private readonly SmartHomeDBContext _dbContext;

    public SessionRepository(SmartHomeDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Session newSession)
    {
        _dbContext.Sessions.Add(newSession);
        _dbContext.SaveChanges();
    }

    public bool HasActiveSessionByEmail(string email)
    {
        return _dbContext.Sessions.Any(s => s.User.Email == email);
    }

    public bool IsAuthenticated(string token)
    {
        return _dbContext.Sessions.Any(s => s.SessionToken == token);
    }

    public User? GetUserByToken(string token)
    {
        return _dbContext.Sessions.Include(s => s.User).ThenInclude(u => u.Role).FirstOrDefault(s => s.SessionToken == token).User;
    }

    public string GetSessionTokenByEmail(string email)
    {
        return _dbContext.Sessions.FirstOrDefault(s => s.User.Email == email)!.SessionToken;
    }

    public bool HasActiveSessionById(Guid userId)
    {
        return _dbContext.Sessions.Include(s => s.User).Any(s => s.UserId == userId);
    }
}
