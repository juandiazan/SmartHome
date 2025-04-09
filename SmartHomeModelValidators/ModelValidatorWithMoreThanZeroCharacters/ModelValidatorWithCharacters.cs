using ModeloValidador.Abstracciones;

namespace ModelValidatorWithMoreThanZeroCharacters;

public class ModelValidatorWithCharacters : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length > 0;
    }
}
