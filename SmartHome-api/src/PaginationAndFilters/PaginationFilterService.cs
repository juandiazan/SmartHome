using Domain;
using PaginationAndFilters.Models;

namespace PaginationAndFilters;
public static class PaginationFilterService
{
    private const int MinimumPossibleValueOfCurrentPage = 1;
    private const int MinimumAmountOfPagesAllowed = 1;

    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public static List<Company> FilterAndPaginateCompanies(
        IQueryable<Company> companies, CompanyFilterArgs args)
    {
        var filteredCompanies = companies
            .Where(company => string.IsNullOrEmpty(args.CompanyName) ||
            company.CompanyName.ToLower().Contains(args.CompanyName.ToLower()))
            .Where(company => string.IsNullOrEmpty(args.CompanyOwnerFullName) ||
            (company.CompanyOwner!.Name + " " + company.CompanyOwner!.Surname).ToLower().Contains(args.CompanyOwnerFullName.ToLower()));

        return Paginate<Company>(filteredCompanies, args.Offset, args.Limit);
    }

    public static List<Device> FilterAndPaginateDevices(
        IQueryable<Device> devices,
        DeviceFilterArgs args)
    {
        var filteredDevices = devices
            .Where(device => string.IsNullOrEmpty(args.DeviceName) ||
            device.DeviceName.ToLower().Contains(args.DeviceName.ToLower()))
            .Where(device => string.IsNullOrEmpty(args.Model) ||
            device.DeviceModel.ToLower().Contains(args.Model.ToLower()))
            .Where(device => string.IsNullOrEmpty(args.CompanyName) ||
            device.CompanyItIsAssociatedTo.CompanyName.ToLower().Contains(args.CompanyName.ToLower()))
            .Where(device => args.DeviceType == null || device.DeviceType == args.DeviceType);

        return Paginate<Device>(filteredDevices, args.Offset, args.Limit);
    }

    public static List<User> FilterAndPaginateUsers(
        IQueryable<User> users, UserFilterArgs args)
    {
        var filteredUsers = users
            .Where(user => string.IsNullOrEmpty(args.Name) || (user.Name + " " + user.Surname).ToLower().Contains(args.Name.ToLower()))
            .Where(user => string.IsNullOrEmpty(args.Role) || user.Role.RoleName.ToLower().Contains(args.Role.ToLower()));

        return Paginate<User>(filteredUsers, args.Offset, args.Limit);
    }

    public static List<Notification> FilterNotifications(
        IQueryable<Notification> notifications, NotificationFilterArgs args)
    {
        var newNotifs = notifications
            .Where(notif => args.DeviceType == null || notif.TriggeringDevice!.Device!.DeviceType == args.DeviceType)
            .Where(notif => args.CreationDate == null || notif.DateTimeOfEvent.Date == args.CreationDate.Value.Date)
            .Where(notif => args.WasRead == null || notif.WasRead == args.WasRead)
            .ToList();

        return newNotifs;
    }

    public static List<HomeDevice> FilterHomeDevices(
        List<HomeDevice> homeDevices, string? room)
    {
        var newHomeDevices = new List<HomeDevice>();

        newHomeDevices = homeDevices
            .Where(hd => string.IsNullOrEmpty(room)
            || (hd.RoomItIsIn is not null && hd.RoomItIsIn!.Name.ToLower().Contains(room.ToLower())))
            .ToList();

        return newHomeDevices;
    }

    public static bool CurrentPageOrPageSizeIsNegative(int? currentPage = 1, int? pageSize = 10)
    {
        return pageSize < MinimumAmountOfPagesAllowed || currentPage < MinimumPossibleValueOfCurrentPage;
    }

    private static List<T> Paginate<T>(IQueryable<T> items, int? page = 1, int? pageSize = 10)
    {
        var ammountSkipped = ((page ?? DefaultPage) - MinimumPossibleValueOfCurrentPage) * (pageSize ?? DefaultPageSize);

        return [.. items
            .Skip(ammountSkipped)
            .Take(pageSize ?? 10)];
    }
}
