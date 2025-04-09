namespace IDataAccess;

public interface IUserHasSessionActive
{
    bool HasActiveSessionById(Guid userId);
}
