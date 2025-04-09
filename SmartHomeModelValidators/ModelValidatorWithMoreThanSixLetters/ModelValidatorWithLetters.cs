using ModeloValidador.Abstracciones;

namespace ModelValidatorWithMoreThanSixLetters;

public class ModelValidatorWithLetters : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Count(char.IsLetter) > 6;
    }
}
