namespace Domain;
public class Role
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string RoleName { get; init; } = null!;
    public ICollection<User> Users { get; init; } = [];
    public ICollection<Permission> Permissions { get; init; } = [];
}
