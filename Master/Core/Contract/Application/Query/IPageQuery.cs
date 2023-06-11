namespace Master.Core.Contract.Application.Query;

public interface IPageQuery<TPayload> : IQuery<TPayload>
{
    int Page { get; set; }
    int Size { get; set; }
    int Skip { get; }
    bool SortAscending { get; set; }
    string SortBy { get; set; }
    bool NeededTotalCount { get; set; }
}