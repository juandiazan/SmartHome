namespace Domain;
public class Permission
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;

    public ICollection<Role> Roles { get; init; } = [];
    public ICollection<Member> Members { get; init; } = [];
}
