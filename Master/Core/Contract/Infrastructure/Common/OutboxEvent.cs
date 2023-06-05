namespace Master.Core.Contract.Infrastructure.Common;

public class OutboxEvent
{
    public long Id { get; set; }
    public Guid EventId { get; set; }
    public string AggregateId { get; set; }
    public string UserId { get; set; }
    public string EventName { get; set; }
    public string EventTypeName { get; set; }
    public string EventPayload { get; set; }
    public string AggregateName { get; set; }
    public string AggregateTypeName { get; set; }
    public DateTime AccuredOn { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public bool IsProccessd { get; set; }
}
