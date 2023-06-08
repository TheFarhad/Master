namespace Master.Utilities.Services.Abstraction.Mapping;

public interface IMap
{
    TOutput Map<TSource, TOutput>(TSource source);
}
