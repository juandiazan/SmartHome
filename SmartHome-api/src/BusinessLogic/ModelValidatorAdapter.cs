using IBusinessLogic;
using ModeloValidador.Abstracciones;

namespace BusinessLogic;
public sealed class ModelValidatorAdapter(IAssemblyLoadingService<IModeloValidador> modelValidatorLoadingService) : IModelValidatorAdapter
{
    public bool IsDeviceModelValid(Guid modelValidatorId, string deviceModel)
    {
        modelValidatorLoadingService.LoadImplementations();

        var modelValidator = modelValidatorLoadingService.GetImplementationById(modelValidatorId);

        if (!modelValidator.EsValido(new Modelo { Value = deviceModel }))
        {
            return false;
        }

        return true;
    }
}
