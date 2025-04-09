using Domain;
using FluentAssertions;
using IBusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using WebApi.Models.Requests;

namespace WebApi.Test;

[TestClass]
public class RoomControllerTest
{
    [TestMethod]
    public void AddHomeDeviceToRoom_WithCorrectData_ShouldReturnOk()
    {
        var roomId = Guid.NewGuid();
        var hardwareId = Guid.NewGuid();
        var request = new AddDeviceToRoomRequest(hardwareId.ToString());

        var homeServiceMock = new Mock<IHomeService>(MockBehavior.Strict);

        homeServiceMock
            .Setup(act => act.AddDeviceToRoomOfHome(roomId, request.HardwareId))
            .Returns(It.Is<Room>(r => r.HomeDevices.Contains(It.Is<HomeDevice>(hd => hd.RoomItIsInId == roomId))));

        var roomController = new RoomController(homeServiceMock.Object);

        var response = roomController.AddHomeDeviceToRoom(roomId, request);

        response.Should().BeOfType<OkObjectResult>();
    }
}
