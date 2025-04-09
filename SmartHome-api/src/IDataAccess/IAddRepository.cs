namespace IDataAccess;
public interface IAddRepository<T>
    where T : class
{
    T Add(T entity);
}
