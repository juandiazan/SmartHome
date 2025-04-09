using Domain;
using DTOs;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Requests;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

[ApiController]
[Route("cameras")]
public sealed class CameraController(
    ICameraService cameraService,
    INotificationService notificationService) : ControllerBase
{
    [HttpPost]
    [CustomAuthorizeFilter("company-owner", "company-owner-home-owner")]
    public IActionResult CreateSecurityCamera([FromBody] CreateCameraRequest request, [FromHeader] string authorization)
    {
        var camera = cameraService.Create(request.ToArgs(), authorization);
        return Created(
            $"cameras/{camera.Id}",
            new
            {
                message = "Camera created successfully",
                CreatedCamera = new CreateCameraResponse(
                    camera.DeviceName,
                    camera.DeviceModel,
                    camera.Description,
                    camera.Photos[0],
                    camera.DeviceType.ToString(),
                    camera.CanBeUsedIndoors,
                    camera.CanBeUsedOutdoors,
                    camera.HasMovementDetectionSupport,
                    camera.HasPersonDetectionSupport)
            });
    }

    [HttpPost("{hardwareId}/motion-detections")]
    public IActionResult DetectMotion(Guid hardwareId)
    {
        var args = new NotificationGenerationArgs(hardwareId, DeviceType.Camera.ToString(), "motion-detected", null);
        notificationService.GenerateAndSendNotification(args);
        return Ok("Motion detected and notifications sent.");
    }

    [HttpPost("{hardwareId}/person-detections")]
    public IActionResult DetectPerson([FromBody] DetectPersonRequest request, [FromRoute] Guid hardwareId)
    {
        var args = new NotificationGenerationArgs(hardwareId, DeviceType.Camera.ToString(), "person-detected", request.IdentifiedPerson);
        notificationService.GenerateAndSendNotification(args);
        return Ok("Person detected and notifications sent.");
    }
}
