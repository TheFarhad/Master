namespace Master.Utilities.Services.Abstraction.Serializing;

public interface ISerialize : IDisposable
{
    string Serialize(object source);
    string Serialize<TSource>(TSource source);
    object? Deserialize(string source, Type type);
    TOutput? Deserialize<TOutput>(string source);
}