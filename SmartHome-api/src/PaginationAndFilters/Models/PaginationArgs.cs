namespace PaginationAndFilters.Models;
public record class PaginationArgs
{
    public int? Offset { get; init; } = 1;
    public int? Limit { get; init; } = 10;

    public PaginationArgs(int? offset = 1, int? limit = 10)
    {
        Offset = offset;
        Limit = limit;
    }
}
