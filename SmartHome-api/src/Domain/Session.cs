namespace Domain;
public sealed class Session
{
    public Guid Id { get; init; }
    public string SessionToken { get; init; } = null!;
    public Guid UserId { get; init; }
    public User User { get; init; } = null!;
}
