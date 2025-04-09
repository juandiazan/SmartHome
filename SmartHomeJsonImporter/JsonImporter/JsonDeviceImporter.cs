using System.Text.Json;
using IImporterService;

namespace JsonImporter;
public class JsonDeviceImporter : IDeviceImporter
{
    public List<DeviceImportDTO> ImportDevices(string path)
    {
        var fileContent = File.ReadAllText(path);

        DevicesListJsonDTO? devicesList;
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            devicesList = JsonSerializer.Deserialize<DevicesListJsonDTO>(fileContent, options);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Error in JSON file format", ex);
        }

        if (devicesList?.Dispositivos == null || devicesList.Dispositivos.Count == 0)
        {
            throw new InvalidOperationException("There are no devices in JSON file");
        }

        var deviceImportList = devicesList.Dispositivos
            .ConvertAll(d =>
            new DeviceImportDTO
            {
                Id = d.Id ?? string.Empty,
                DeviceType = SetDeviceType(d.Tipo ?? string.Empty),
                DeviceName = d.Nombre ?? string.Empty,
                DeviceModel = d.Modelo ?? string.Empty,
                Photos = d.Fotos?.ConvertAll(f =>
                new DevicePictureDTO
                {
                    Path = f.Path ?? string.Empty,
                    IsMain = f.EsPrincipal ?? false
                }) ?? [],
                HasPersonDetection = d.Person_detection,
                HasMovementDetection = d.Movement_detection
            });

        return deviceImportList;
    }

    private string SetDeviceType(string deviceTypeFromJson)
    {
        deviceTypeFromJson = deviceTypeFromJson.ToLower();

        switch (deviceTypeFromJson)
        {
            case "camera":
                return "Camera";
            case "sensor-open-close":
                return "Sensor";
            case "sensor-movement":
                return "MovementSensor";

            default:
                return string.Empty;
        }
    }
}
