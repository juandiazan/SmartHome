namespace DTOs;
public sealed record class CreateCompanyArgs
{
    public string CompanyName { get; init; }
    public string Logotype { get; init; }
    public string Rut { get; init; }
    public string DeviceModelValidationId { get; init; }

    public CreateCompanyArgs(
        string companyName,
        string logotype,
        string rut,
        string deviceModelValidationId)
    {
        if (HasEmptyCompanyName(companyName))
        {
            throw new ArgumentNullException(null, "Company name cannot be empty");
        }

        if (HasEmptyLogotype(logotype))
        {
            throw new ArgumentNullException(null, "Company logotype cannot be empty");
        }

        if (HasEmptyRut(rut))
        {
            throw new ArgumentNullException(null, "Company rut cannot be empty");
        }

        if (IsDeviceModelValidatorIdInvalid(deviceModelValidationId))
        {
            throw new FormatException("Invalid device model validator format");
        }

        CompanyName = companyName;
        Logotype = logotype;
        Rut = rut;
        DeviceModelValidationId = deviceModelValidationId;
    }

    private static bool HasEmptyCompanyName(string companyName)
    {
        return string.IsNullOrEmpty(companyName);
    }

    private static bool HasEmptyLogotype(string logotype)
    {
        return string.IsNullOrEmpty(logotype);
    }

    private static bool HasEmptyRut(string rut)
    {
        return string.IsNullOrEmpty(rut);
    }

    private static bool IsDeviceModelValidatorIdInvalid(string deviceModelValidatorId)
    {
        return HasEmptyModelValidatorId(deviceModelValidatorId) || IsNotAnId(deviceModelValidatorId);
    }

    private static bool HasEmptyModelValidatorId(string deviceModelValidatorId)
    {
        return string.IsNullOrEmpty(deviceModelValidatorId);
    }

    private static bool IsNotAnId(string deviceModelValidatorId)
    {
        return deviceModelValidatorId == Guid.Empty.ToString() || !Guid.TryParse(deviceModelValidatorId, out _);
    }
}
