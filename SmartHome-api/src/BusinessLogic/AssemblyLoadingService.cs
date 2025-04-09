using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DTOs;
using IBusinessLogic;

namespace BusinessLogic;

[ExcludeFromCodeCoverage]
public sealed class AssemblyLoadingService<TIInterface> : IAssemblyLoadingService<TIInterface>
        where TIInterface : class
{
    private readonly DirectoryInfo directory;
    private List<Type> implementations = [];

    public AssemblyLoadingService(string path)
    {
        directory = new(path);
        LoadImplementations();
    }

    public void LoadImplementations()
    {
        var files = directory
            .GetFiles("*.dll")
            .ToList();

        implementations = [];
        files.ForEach(file =>
        {
            var assemblyLoaded = Assembly.LoadFile(file.FullName);
            var loadedTypes = assemblyLoaded
            .GetTypes()
            .Where(t => t.IsClass && typeof(TIInterface).IsAssignableFrom(t))
            .ToList();

            implementations = implementations
            .Union(loadedTypes)
            .ToList();
        });
    }

    public List<GetAllImplementationsArgs> GetImplementations()
    {
        var returnedImplementations = new List<GetAllImplementationsArgs>();

        implementations.ForEach(imp =>
        {
            returnedImplementations.Add(new GetAllImplementationsArgs(imp.GUID.ToString(), imp.Name));
        });

        return returnedImplementations;
    }

    public TIInterface GetImplementationById(Guid id, params object[] args)
    {
        if (!Exists(id))
        {
            throw new KeyNotFoundException("Model validator could not be found");
        }

        var type = implementations.First(imp => imp.GUID == id);

        return Activator.CreateInstance(type, args) as TIInterface;
    }

    private bool Exists(Guid id)
    {
        return implementations.Any(imp => imp.GUID == id);
    }
}
