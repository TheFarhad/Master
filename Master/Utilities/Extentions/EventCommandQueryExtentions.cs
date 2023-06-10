namespace Master.Utilities.Extentions;

using Core.Domain.Aggregate.Event;
using Core.Contract.Application.Command;
using Core.Contract.Application.Query;

public static class EventCommandQueryExtentions
{
    public static Type Type(this IEvent source) => source.Type();
    public static Type Type(this ICommand source) => source.Type();
    public static Type Type<TPayload>(this ICommand<TPayload> source) => source.Type();
    public static Type Type<TPayload>(this IQuery<TPayload> source) => source.Type();
}
