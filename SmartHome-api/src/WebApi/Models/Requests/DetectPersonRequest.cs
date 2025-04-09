namespace WebApi.Models.Requests;

public sealed class DetectPersonRequest
{
    public string? IdentifiedPerson { get; init; }

    public DetectPersonRequest(string? identifiedPerson)
    {
        IdentifiedPerson = identifiedPerson;
    }
}
