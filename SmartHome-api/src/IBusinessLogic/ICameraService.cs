using Domain;
using DTOs;

namespace IBusinessLogic;

public interface ICameraService
{
    Camera Create(CreateCameraArgs args, string auth);
}
