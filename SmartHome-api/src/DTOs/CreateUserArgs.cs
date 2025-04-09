using System.Text.RegularExpressions;

namespace DTOs;

public record class CreateUserArgs
{
    private const int MinimumPasswordLength = 6;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public CreateUserArgs(
        string name,
        string surname,
        string email,
        string password)
    {
        if (IsNameInvalid(name))
        {
            throw new ArgumentNullException(null, "Administrator name cannot be null or empty");
        }

        if (IsSurnameInvalid(surname))
        {
            throw new ArgumentNullException(null, "Administrator surname cannot be null or empty");
        }

        if (IsEmailEmpty(email))
        {
            throw new ArgumentNullException(null, "Administrator email cannot be null or empty");
        }

        if (IsEmailFormatInvalid(email))
        {
            throw new FormatException("Administrator email invalid format");
        }

        if (PasswordIsNullOrEmpty(password))
        {
            throw new ArgumentNullException(null, "Password cannot be null");
        }

        if (PassworLengthIsLessThanMinimum(password))
        {
            throw new FormatException("Administrator password length should be at least six");
        }

        if (PasswordDoesNotHaveAtLeastOneSpecialChar(password))
        {
            throw new FormatException("The password must have at least one especial character");
        }

        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }

    private static bool PasswordDoesNotHaveAtLeastOneSpecialChar(string argsPassword)
    {
        return !(argsPassword.Contains('!')
            || argsPassword.Contains('@')
            || argsPassword.Contains('#')
            || argsPassword.Contains('$')
            || argsPassword.Contains('%')
            || argsPassword.Contains('&')
            || argsPassword.Contains('*'));
    }

    private static bool PassworLengthIsLessThanMinimum(string adminPassword)
    {
        return adminPassword.Length < MinimumPasswordLength;
    }

    private static bool PasswordIsNullOrEmpty(string password)
    {
        return string.IsNullOrEmpty(password);
    }

    private static bool IsEmailFormatInvalid(string argsEmail)
    {
        var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com)$";
        return !Regex.IsMatch(argsEmail, pattern);
    }

    private static bool IsEmailEmpty(string email)
    {
        return string.IsNullOrEmpty(email);
    }

    private static bool IsSurnameInvalid(string surname)
    {
        return string.IsNullOrEmpty(surname);
    }

    private static bool IsNameInvalid(string name)
    {
        return string.IsNullOrEmpty(name);
    }
}
