using System.Diagnostics.CodeAnalysis;
using ImporterService;

namespace BusinessLogic.Test;

[ExcludeFromCodeCoverage]
public class TestDeviceImporter : IDeviceImporter
{
    public List<DeviceImportDTO> ImportDevices(string path)
    {
        return
        [
            new DeviceImportDTO
            {
                DeviceType = "Sensor",
                DeviceName = "Test Camera",
                DeviceModel = "Model X",
                Photos =
                [
                    new DevicePictureDTO { Path = "test.jpg", IsMain = true }
                ],
                HasMovementDetection = true,
                HasPersonDetection = false
            }

        ];
    }
}
