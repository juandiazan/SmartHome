namespace WebApi.Models.Requests;

public sealed class AddAliasToHomeRequest
{
    public string Alias { get; init; }

    public AddAliasToHomeRequest(string alias)
    {
        Alias = alias;
    }
}
