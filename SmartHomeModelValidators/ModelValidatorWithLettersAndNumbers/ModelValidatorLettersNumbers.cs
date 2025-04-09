using ModeloValidador.Abstracciones;

namespace ModelValidatorWithLettersAndNumbers;

public class ModelValidatorLettersNumbers : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length == 6 &&
            modelo.Value.Take(3).All(char.IsLetter) &&
            modelo.Value.Skip(3).Take(3).All(char.IsDigit);
    }
}
