using System.Linq.Expressions;

namespace IDataAccess;
public interface IAddExistsRepository<T> : IAddRepository<T>
    where T : class
{
    bool Exists(Expression<Func<T, bool>> predicate);
}
