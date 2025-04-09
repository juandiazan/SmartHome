using IBusinessLogic;

namespace BusinessLogic;
public sealed class PathValidator : IPathValidator
{
    public bool PathExists(string path)
    {
        return File.Exists(path);
    }
}
