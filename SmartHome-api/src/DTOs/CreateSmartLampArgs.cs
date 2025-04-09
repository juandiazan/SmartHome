namespace DTOs;

public sealed record class CreateSmartLampArgs : CreateDeviceArgs
{
    public bool IsTurnedOn { get; set; }

    public CreateSmartLampArgs(
        string smartLampName,
        string smartLampModel,
        string description,
        List<string> photos,
        bool isTurnedOn,
        string deviceType)
        : base(smartLampName, smartLampModel, description, photos, deviceType)
    {
        IsTurnedOn = isTurnedOn;
    }
}
