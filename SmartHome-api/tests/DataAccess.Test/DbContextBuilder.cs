using DataAccess.DBContext;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Test;

internal sealed class DbContextBuilder
{
    private static readonly SqliteConnection _connection = new("Data Source=:memory:");

    public static SmartHomeDBContext BuildTestDbContext()
    {
        var options = new DbContextOptionsBuilder<SmartHomeDBContext>()
            .UseSqlite(_connection)
        .Options;
        _connection.Open();

        var context = new SmartHomeDBContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
