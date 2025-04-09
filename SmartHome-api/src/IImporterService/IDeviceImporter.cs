namespace ImporterService;

public interface IDeviceImporter
{
    List<DeviceImportDTO> ImportDevices(string path);
}
