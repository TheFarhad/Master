namespace Master.Core.Contract.Application.Query;

public class PageQuery<TPayload> : IPageQuery<TPayload>
{
    public int Page { get; set; }
    public int Size { get; set; } = 10;
    public int Skip => (Page - 1) * Size;
    public bool SortAscending { get; set; } = true;
    public string SortBy { get; set; }
    public bool NeededTotalCount { get; set; } = false;
}