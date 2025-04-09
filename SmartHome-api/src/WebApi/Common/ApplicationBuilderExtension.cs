using System.Diagnostics.CodeAnalysis;
using BusinessLogic;
using DataAccess;
using Domain;
using IBusinessLogic;
using IDataAccess;
using ImporterService;
using ModeloValidador.Abstracciones;
using NotificationStrategies;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtension
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IAddRepository<Camera>, CameraRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IHomeDeviceRepository, HomeDeviceRepository>();
        services.AddScoped<IHomeOwnerRepository, HomeOwnerRepository>();
        services.AddScoped<IHomeRepository, HomeRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserHasSessionActive, SessionRepository>();
        services.AddScoped<IAdministratorRepository, UserRepository>();
        services.AddScoped<ICompanyOwnerRepository, CompanyOwnerRepository>();
        services.AddScoped<ISmartLampRepository, SmartLampRepository>();

        return services;
    }

    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ICameraService, CameraService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IHomeDeviceService, HomeDeviceService>();
        services.AddScoped<IHomeOwnerService, HomeOwnerService>();
        services.AddScoped<IHomeService, HomeService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<INotificationCreator, NotificationCreator>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanyOwnerService, CompanyOwnerService>();
        services.AddScoped<ISmartLampService, SmartLampService>();

        return services;
    }

    public static IServiceCollection AddUtilsServices(this IServiceCollection services)
    {
        services.AddScoped<IPathValidator, PathValidator>();
        services.AddScoped<IModelValidatorAdapter, ModelValidatorAdapter>();
        services.AddScoped<INotificationStrategyManager, NotificationStrategyManager>();
        services.AddScoped<INotificationStrategy, PersonDetectionNotificationStrategy>();
        services.AddScoped<INotificationStrategy, MovementDetectionNotificationStrategy>();
        services.AddScoped<INotificationStrategy, WindowSensorOpenedNotificationStrategy>();
        services.AddScoped<INotificationStrategy, WindowSensorClosedNotificationStrategy>();

        EnsureDirectoryExists(AppDomain.CurrentDomain.BaseDirectory + "Assemblies");

        services.AddScoped<IAssemblyLoadingService<IModeloValidador>, AssemblyLoadingService<IModeloValidador>>(provider =>
        {
            var assembliesRoute = AppDomain.CurrentDomain.BaseDirectory + "Assemblies";
            return new AssemblyLoadingService<IModeloValidador>(assembliesRoute);
        });

        EnsureDirectoryExists(AppDomain.CurrentDomain.BaseDirectory + "Importers");

        services.AddScoped<IAssemblyLoadingService<IDeviceImporter>, AssemblyLoadingService<IDeviceImporter>>(provider =>
        {
            var assembliesRoute = AppDomain.CurrentDomain.BaseDirectory + "Importers";
            return new AssemblyLoadingService<IDeviceImporter>(assembliesRoute);
        });

        return services;
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
