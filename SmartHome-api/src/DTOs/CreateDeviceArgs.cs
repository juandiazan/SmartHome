namespace DTOs;
public record CreateDeviceArgs
{
    private const int MinimumAmountOfPhotos = 0;

    public string DeviceName { get; init; }
    public string DeviceModel { get; init; }
    public string Description { get; init; }
    public List<string> Photos { get; init; }
    public string DeviceType { get; init; }

    public CreateDeviceArgs(string deviceName, string deviceModel, string description, List<string> photos, string deviceType)
    {
        if (IsDeviceNameNullOrEmpty(deviceName))
        {
            throw new ArgumentNullException(null, "Device name cannot be empty");
        }

        if (IsDeviceModelNullOrEmpty(deviceModel))
        {
            throw new ArgumentNullException(null, "Device model cannot be empty");
        }

        if (IsDescriptionNullOrEmpty(description))
        {
            throw new ArgumentNullException(null, "Device description cannot be empty");
        }

        if (IsPhotosNull(photos))
        {
            throw new ArgumentNullException(null, "Device photos cannot be null or empty");
        }

        DeviceName = deviceName;
        DeviceModel = deviceModel;
        Description = description;
        Photos = photos;
        DeviceType = deviceType;
    }

    private static bool IsDeviceNameNullOrEmpty(string deviceName)
    {
        return string.IsNullOrEmpty(deviceName);
    }

    private static bool IsDeviceModelNullOrEmpty(string deviceModel)
    {
        return string.IsNullOrEmpty(deviceModel);
    }

    private static bool IsDescriptionNullOrEmpty(string description)
    {
        return string.IsNullOrEmpty(description);
    }

    private static bool IsPhotosNull(List<string> photos)
    {
        return photos == null || photos.Count <= MinimumAmountOfPhotos;
    }
}
