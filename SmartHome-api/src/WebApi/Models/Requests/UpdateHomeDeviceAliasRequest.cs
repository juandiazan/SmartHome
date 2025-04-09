using DTOs;

namespace WebApi.Models.Requests;

public sealed class UpdateHomeDeviceAliasRequest
{
    public string? HardwareId { get; set; }
    public string? Alias { get; init; }

    public UpdateHomeDeviceAliasRequest(string hardwareId, string alias)
    {
        HardwareId = hardwareId;
        Alias = alias;
    }

    public UpdateHomeDeviceArgs ToArgs()
    {
        return new UpdateHomeDeviceArgs(HardwareId ?? string.Empty, Alias ?? string.Empty);
    }
}
