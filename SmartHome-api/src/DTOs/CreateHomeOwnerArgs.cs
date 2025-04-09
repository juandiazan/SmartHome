namespace DTOs;
public sealed record class CreateHomeOwnerArgs : CreateUserArgs
{
    public string ProfilePicture { get; init; }

    public CreateHomeOwnerArgs(
        string name,
        string surname,
        string profilePicture,
        string email,
        string password)
        : base(name, surname, email, password)
    {
        if (IsProfilePictureNullOrEmpty(profilePicture))
        {
            throw new ArgumentNullException(null, "Profile picture cannot be null.");
        }

        ProfilePicture = profilePicture;
    }

    private static bool IsProfilePictureNullOrEmpty(string profilePicture)
    {
        return string.IsNullOrEmpty(profilePicture);
    }
}
