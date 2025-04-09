using DTOs;

namespace IBusinessLogic;
public interface IAssemblyLoadingService<TIInterface>
{
    void LoadImplementations();
    List<GetAllImplementationsArgs> GetImplementations();
    TIInterface GetImplementationById(Guid id, params object[] args);
}
