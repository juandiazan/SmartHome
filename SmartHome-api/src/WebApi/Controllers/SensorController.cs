using Domain;
using DTOs;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("sensors")]
public sealed class SensorController(
    IDeviceService sensorService,
    INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult CreateSensor([FromBody] CreateDeviceRequest request, [FromHeader] string authorization)
    {
        var sensor = sensorService.Create(request.ToArgs(), authorization);

        return Created($"sensors/{sensor.Id}",
            new
            {
                message = "Sensor created successfully",
                CreatedSensor = new CreateSensorResponse(
                    sensor.DeviceName,
                    sensor.DeviceModel,
                    sensor.Description,
                    sensor.Photos[0],
                    sensor.DeviceType.ToString())
            });
    }

    [HttpPost("{hardwareId}/opened-notifications")]
    public IActionResult CreateWindowOpenedNotification([FromRoute] Guid hardwareId)
    {
        var args = new NotificationGenerationArgs(hardwareId, DeviceType.Sensor.ToString(), "window-opened", null);
        notificationService.GenerateAndSendNotification(args);
        return Ok("Window opened notification sent.");
    }

    [HttpPost("{hardwareId}/closed-notifications")]
    public IActionResult CreateWindowClosedNotification([FromRoute] Guid hardwareId)
    {
        var args = new NotificationGenerationArgs(hardwareId, DeviceType.Sensor.ToString(), "window-closed", null);
        notificationService.GenerateAndSendNotification(args);
        return Ok("Window closed notification sent.");
    }

    [HttpPost("{hardwareId}/motion-detections")]
    public IActionResult DetectMovementSensorMotion([FromRoute] Guid hardwareId)
    {
        var args = new NotificationGenerationArgs(hardwareId, DeviceType.MovementSensor.ToString(), "motion-detected", null);
        notificationService.GenerateAndSendNotification(args);
        return Ok("Motion detected and notifications sent.");
    }
}
