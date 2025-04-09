using ModeloValidador.Abstracciones;

namespace ModelValidatorWithMoreThanOneLetter;

public class ModelValidatorMoreThanOne : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Count(char.IsLetter) > 1;
    }
}
