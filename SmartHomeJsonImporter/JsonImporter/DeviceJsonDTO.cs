namespace JsonImporter;

public sealed record class DeviceJsonDTO
{
    public string? Id { get; init; }
    public string? Tipo { get; init; }
    public string? Nombre { get; init; }
    public string? Modelo { get; init; }
    public List<DevicePictureJsonDTO>? Fotos { get; init; }
    public bool? Person_detection { get; init; }
    public bool? Movement_detection { get; init; }

    public DeviceJsonDTO(
        string? id,
        string? tipo,
        string? nombre,
        string? modelo,
        List<DevicePictureJsonDTO>? fotos,
        bool? person_detection,
        bool? movement_detection)
    {
        Id = id;
        Tipo = tipo;
        Nombre = nombre;
        Modelo = modelo;
        Fotos = fotos;
        Person_detection = person_detection;
        Movement_detection = movement_detection;
    }
}
