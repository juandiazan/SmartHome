using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContext;

public class SmartHomeDBContext(DbContextOptions options)
    : DbContext(options)
{
    public static readonly Guid AdministratorRoleId = Guid.Parse("6321a816-3080-1001-aab7-5032779c3714");
    public static readonly Guid CreateAdminPermissionId = Guid.Parse("6321a816-3080-1002-aab7-5032779c3714");
    public static readonly Guid CreateCompanyOwnerPermissionId = Guid.Parse("6321a816-3080-1003-aab7-5032779c3714");
    public static readonly Guid ListUsersPermissionId = Guid.Parse("6321a816-3080-1004-aab7-5032779c3714");
    public static readonly Guid ListCompaniesPermissionId = Guid.Parse("6321a816-3080-1005-aab7-5032779c3714");

    public static readonly Guid CompanyOwnerRoleId = Guid.Parse("6321a816-3080-2001-aab7-5032779c3714");
    public static readonly Guid CreateCompanyPermissionId = Guid.Parse("6321a816-3080-2002-aab7-5032779c3714");
    public static readonly Guid CreateCameraPermissionId = Guid.Parse("6321a816-3080-2003-aab7-5032779c3714");
    public static readonly Guid CreateSensorPermissionId = Guid.Parse("6321a816-3080-2004-aab7-5032779c3714");

    public static readonly Guid HomeOwnerRoleId = Guid.Parse("6321a816-3080-3001-aab7-5032779c3714");
    public static readonly Guid CreateHomePermissionId = Guid.Parse("6321a816-3080-3002-aab7-5032779c3714");
    public static readonly Guid AddMemberPermissionId = Guid.Parse("6321a816-3080-3003-aab7-5032779c3714");
    public static readonly Guid AddDevicePermissionId = Guid.Parse("6321a816-3080-3004-aab7-5032779c3714");
    public static readonly Guid ListMembersPermissionId = Guid.Parse("6321a816-3080-3005-aab7-5032779c3714");
    public static readonly Guid ListDevicesPermissionId = Guid.Parse("6321a816-3080-3006-aab7-5032779c3714");
    public static readonly Guid GivePermissionsPermissionId = Guid.Parse("6321a816-3080-3007-aab7-5032779c3714");

    private static readonly Guid HomeSpecificPermissionsRole = Guid.Parse("6321a816-3080-4001-aab7-5032779c3714"); // Dummy Role for default
    public static readonly Guid AddDeviceToSpecificHomePermissionId = Guid.Parse("6321a816-3080-4002-aab7-5032779c3714");
    public static readonly Guid ListDevicesOfSpecificHomePermissionId = Guid.Parse("6321a816-3080-4003-aab7-5032779c3714");
    public static readonly Guid ReceiveNotificationsPermissionId = Guid.Parse("6321a816-3080-4004-aab7-5032779c3714");
    public static readonly Guid ChangeHomeDeviceAliasPermissionId = Guid.Parse("6321a816-3080-4005-aab7-5032779c3714");

    public static readonly Guid AdministratorHomeOwnerRoleId = Guid.Parse("6321a816-3080-5001-aab7-5032779c3714");
    public static readonly Guid CompanyOwnerHomeOwnerRoleId = Guid.Parse("6321a816-3080-5002-aab7-5032779c3714");

    private readonly Guid administratorId = Guid.Parse("00000001-6618-4bab-a6b6-9a32a11893f8");

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;

    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<CompanyOwner> CompanyOwners { get; set; } = null!;

    public DbSet<HomeOwner> HomeOwners { get; set; } = null!;
    public DbSet<Device> Devices { get; set; } = null!;
    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<HomeDevice> HomeDevices { get; set; } = null!;
    public DbSet<Home> Homes { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;

    public DbSet<Room> Rooms { get; set; } = null!;

    public DbSet<Session> Sessions { get; set; } = null!;

    public DbSet<SmartLamp> SmartLamps { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigModel(modelBuilder);
        AddDefaultData(modelBuilder);
    }

    private void ConfigModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(e => e.ToTable("RolePermissions"));

        modelBuilder.Entity<User>().HasAlternateKey(co => co.Email);
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithOne(u => u.Session)
            .HasForeignKey<Session>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasDiscriminator<string>("UserType")
            .HasValue<User>("administrator")
            .HasValue<HomeOwner>("homeowner")
            .HasValue<CompanyOwner>("companyowner");

        modelBuilder.Entity<CompanyOwner>()
            .HasOne(c => c.AssociatedCompany)
            .WithOne(co => co.CompanyOwner)
            .HasForeignKey<CompanyOwner>(co => co.AssociatedCompanyId)
            .IsRequired(false);

        modelBuilder.Entity<Company>().HasAlternateKey(k => k.Rut);

        modelBuilder.Entity<Device>()
            .HasDiscriminator<string>("TypeOfDevice")
            .HasValue<Device>("sensor")
            .HasValue<Camera>("camera")
            .HasValue<SmartLamp>("smartlamp");

        modelBuilder.Entity<Device>()
            .HasOne(d => d.CompanyItIsAssociatedTo)
            .WithMany(c => c.AssociatedDevices)
            .HasForeignKey(d => d.CompanyId)
            .IsRequired(true);

        modelBuilder.Entity<HomeDevice>().HasKey(k => k.HardwareId);
        modelBuilder.Entity<HomeDevice>()
            .HasOne(hd => hd.Device)
            .WithMany(d => d.HomeDevices)
            .HasForeignKey(hd => hd.DeviceId)
            .IsRequired(true);

        modelBuilder.Entity<Home>().OwnsOne(h => h.Location, loc =>
        {
            loc.Property(l => l.Longitude).IsRequired();
            loc.Property(l => l.Latitude).IsRequired();
        });

        modelBuilder.Entity<Home>().OwnsOne(h => h.Address, addr =>
        {
            addr.Property(a => a.MainStreet).IsRequired();
            addr.Property(a => a.DoorNumber).IsRequired();
        });

        modelBuilder.Entity<Room>()
            .HasOne(r => r.HomeItBelongsTo)
            .WithMany(h => h.Rooms)
            .HasForeignKey(fk => fk.HomeItBelongsToId)
            .IsRequired(true);

        modelBuilder.Entity<HomeDevice>()
            .HasOne(hd => hd.RoomItIsIn)
            .WithMany(r => r.HomeDevices)
            .HasForeignKey(hd => hd.RoomItIsInId)
            .IsRequired(false);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.AssociatedHomeOwner)
            .WithMany(ho => ho.Members)
            .HasForeignKey(m => m.AssociatedHomeOwnerId);

        modelBuilder.Entity<Member>()
            .HasMany(p => p.Permissions)
            .WithMany(p => p.Members)
            .UsingEntity(e => e.ToTable("MemberPermissions"));
    }

    private void AddDefaultData(ModelBuilder modelBuilder)
    {
        var administratorRole = new Role
        {
            Id = AdministratorRoleId,
            RoleName = "administrator"
        };

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = CreateAdminPermissionId, Name = "create-administrator" },
            new Permission { Id = CreateCompanyOwnerPermissionId, Name = "create-companyowner" },
            new Permission { Id = ListUsersPermissionId, Name = "list-users" },
            new Permission { Id = ListCompaniesPermissionId, Name = "list-companies" },
            new Permission { Id = CreateCompanyPermissionId, Name = "create-company" }, // Beginning of CompanyOwner permissions
            new Permission { Id = CreateCameraPermissionId, Name = "create-camera" },
            new Permission { Id = CreateSensorPermissionId, Name = "create-sensor" },
            new Permission { Id = CreateHomePermissionId, Name = "create-home" }, // Beginning of HomeOwner permissions
            new Permission { Id = AddMemberPermissionId, Name = "add-member-to-home" },
            new Permission { Id = AddDevicePermissionId, Name = "add-device-to-home" },
            new Permission { Id = ListMembersPermissionId, Name = "list-members-of-home" },
            new Permission { Id = ListDevicesPermissionId, Name = "list-devices-of-home" },
            new Permission { Id = GivePermissionsPermissionId, Name = "add-permissions-to-member" },
            new Permission { Id = AddDeviceToSpecificHomePermissionId, Name = "add-device-to-specific-home" }, // Beginning of Home-Specific permissions
            new Permission { Id = ListDevicesOfSpecificHomePermissionId, Name = "list-devices-of-specific-home" },
            new Permission { Id = ReceiveNotificationsPermissionId, Name = "receive-notifications" },
            new Permission { Id = ChangeHomeDeviceAliasPermissionId, Name = "change-alias-of-specific-device" });

        modelBuilder.Entity<Role>().HasData(
            administratorRole,
            new Role
            {
                Id = CompanyOwnerRoleId,
                RoleName = "company-owner"
            },
            new Role
            {
                Id = HomeOwnerRoleId,
                RoleName = "home-owner"
            },
            new Role
            {
                Id = HomeSpecificPermissionsRole,
                RoleName = "home-specific-permissions"
            },
            new Role
            {
                Id = AdministratorHomeOwnerRoleId,
                RoleName = "admin-home-owner"
            },
            new Role
            {
                Id = CompanyOwnerHomeOwnerRoleId,
                RoleName = "company-owner-home-owner"
            });

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<Dictionary<string, object>>(
            "RolePermissions",
            r => r.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
            p => p.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
            join =>
            {
                join.HasData(
                    new { RoleId = AdministratorRoleId, PermissionId = CreateAdminPermissionId },
                    new { RoleId = AdministratorRoleId, PermissionId = CreateCompanyOwnerPermissionId },
                    new { RoleId = AdministratorRoleId, PermissionId = ListUsersPermissionId },
                    new { RoleId = AdministratorRoleId, PermissionId = ListCompaniesPermissionId },
                    new { RoleId = CompanyOwnerRoleId, PermissionId = CreateCompanyPermissionId }, // Beginning of CompanyOwner permissions
                    new { RoleId = CompanyOwnerRoleId, PermissionId = CreateCameraPermissionId },
                    new { RoleId = CompanyOwnerRoleId, PermissionId = CreateSensorPermissionId },
                    new { RoleId = HomeOwnerRoleId, PermissionId = CreateHomePermissionId }, // Beginning of HomeOwner permissions
                    new { RoleId = HomeOwnerRoleId, PermissionId = AddMemberPermissionId },
                    new { RoleId = HomeOwnerRoleId, PermissionId = AddDevicePermissionId },
                    new { RoleId = HomeOwnerRoleId, PermissionId = ListMembersPermissionId },
                    new { RoleId = HomeOwnerRoleId, PermissionId = ListDevicesPermissionId },
                    new { RoleId = HomeOwnerRoleId, PermissionId = GivePermissionsPermissionId },
                    new { RoleId = HomeSpecificPermissionsRole, PermissionId = AddDeviceToSpecificHomePermissionId }, // Beginning of Home-Specific permissions
                    new { RoleId = HomeSpecificPermissionsRole, PermissionId = ListDevicesOfSpecificHomePermissionId },
                    new { RoleId = HomeSpecificPermissionsRole, PermissionId = ReceiveNotificationsPermissionId },
                    new { RoleId = HomeSpecificPermissionsRole, PermissionId = ChangeHomeDeviceAliasPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = CreateAdminPermissionId }, // Beginning of Admin-Home-Owner permissions
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = CreateCompanyOwnerPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = ListUsersPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = ListCompaniesPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = CreateHomePermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = AddMemberPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = AddDevicePermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = ListMembersPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = ListDevicesPermissionId },
                    new { RoleId = AdministratorHomeOwnerRoleId, PermissionId = GivePermissionsPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = CreateCompanyPermissionId }, // Beginning of CompanyOwner-HomeOwner permissions
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = CreateCameraPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = CreateSensorPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = CreateHomePermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = AddMemberPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = AddDevicePermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = ListMembersPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = ListDevicesPermissionId },
                    new { RoleId = CompanyOwnerHomeOwnerRoleId, PermissionId = GivePermissionsPermissionId });
            });

        modelBuilder.Entity<User>()
            .HasData(
            new User
            {
                Id = administratorId,
                Name = "User",
                Surname = "Admin",
                Email = "administrator@gmail.com",
                Password = "admin123!",
                CreationDate = new DateTime(2024, 10, 8, 12, 0, 0),
                RoleId = AdministratorRoleId,
            });
    }
}
