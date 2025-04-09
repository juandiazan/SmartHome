namespace DTOs;

public sealed record class AddMemberToHomeArgs
{
    public string EmailOfNewMember { get; init; }
    public bool CanAddDeviceToHome { get; init; }
    public bool CanSeeDevicesOfHome { get; init; }
    public bool CanChangeAliasOfDevices { get; init; }

    public AddMemberToHomeArgs(string emailOfNewMember, bool canAddDeviceToHome, bool canSeeDeviceToHome, bool canChangeAliasOfDevices)
    {
        EmailOfNewMember = emailOfNewMember;
        CanAddDeviceToHome = canAddDeviceToHome;
        CanSeeDevicesOfHome = canSeeDeviceToHome;
        CanChangeAliasOfDevices = canChangeAliasOfDevices;
    }
}
