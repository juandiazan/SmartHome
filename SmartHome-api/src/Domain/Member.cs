namespace Domain;

public class Member
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<Permission> Permissions { get; set; } = [];
    public Guid AssociatedHomeOwnerId { get; set; }
    public User AssociatedHomeOwner { get; set; } = null!;
    public Member(List<Permission> permissions, User associatedHomeOwner)
    {
        Permissions = permissions;
        AssociatedHomeOwner = associatedHomeOwner;
    }

    public Member()
    {
    }
}
