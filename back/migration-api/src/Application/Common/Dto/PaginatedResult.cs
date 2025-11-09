namespace Application.Common.Dto;

public class PaginatedResult<T>
{
    public IEnumerable<T> PageItems { get; set; } = Enumerable.Empty<T>();
    public long TotalCount { get; set; }
}
