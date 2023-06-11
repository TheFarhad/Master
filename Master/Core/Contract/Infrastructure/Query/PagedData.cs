namespace Master.Core.Contract.Infrastructure.Query;

public class PagedData<T>
{
    public List<T>? QueryResult { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public int TotalCount { get; set; }
}
