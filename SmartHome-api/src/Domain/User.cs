namespace Domain;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public DateTime CreationDate { get; init; } = DateTime.Now;

    public Session? Session { get; set; } = null;

    public ICollection<Member> Members { get; set; } = [];
}
