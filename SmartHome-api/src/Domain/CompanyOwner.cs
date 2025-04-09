namespace Domain;
public sealed class CompanyOwner : User
{
    public Guid? AssociatedCompanyId = null;
    public Company? AssociatedCompany { get; set; }
    public bool AccountState { get; set; }
}
