using DTOs;

namespace WebApi.Models.Requests;

public sealed class NewMemberToAddRequest
{
    public string? EmailOfNewMember { get; init; }
    public bool CanAddDeviceToHome { get; init; }
    public bool CanSeeDevicesOfHome { get; init; }
    public bool CanChangeAliasOfDevices { get; init; }

    public NewMemberToAddRequest(string emailOfNewMember, bool canAddDeviceToHome, bool canSeeDevicesOfHome, bool canChangeAliasOfDevices)
    {
        EmailOfNewMember = emailOfNewMember;
        CanAddDeviceToHome = canAddDeviceToHome;
        CanSeeDevicesOfHome = canSeeDevicesOfHome;
        CanChangeAliasOfDevices = canChangeAliasOfDevices;
    }

    public AddMemberToHomeArgs ToArgs()
    {
        var newArgs = new AddMemberToHomeArgs(
            EmailOfNewMember ?? string.Empty,
            CanAddDeviceToHome,
            CanSeeDevicesOfHome,
            CanChangeAliasOfDevices);
        return newArgs;
    }
}
