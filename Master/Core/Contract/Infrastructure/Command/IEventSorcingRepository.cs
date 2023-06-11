namespace Master.Core.Contract.Infrastructure.Command;

public interface IEventSorcingRepository
{
    public List<OutboxEvent> Get(int maxCount = 100);
    void MarkAsRead(List<OutboxEvent> outBoxEventItems);
}

