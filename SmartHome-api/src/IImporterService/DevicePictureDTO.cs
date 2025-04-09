namespace ImporterService;
public sealed record class DevicePictureDTO
{
    public string Path { get; init; } = null!;
    public bool IsMain { get; init; }
}
