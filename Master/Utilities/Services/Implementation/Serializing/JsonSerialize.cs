namespace Master.Utilities.Services.Implementation.Serializing;

using System.Text.Json;
using Microsoft.Extensions.Logging;
using Extentions;
using Abstraction.Serializing;

public class JsonSerialize : ISerialize
{
    private readonly ILogger<NewtonSoftSerialize> _logger;
    private readonly JsonSerializerOptions _options;

    public JsonSerialize(ILogger<NewtonSoftSerialize> logger)
    {
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _logger = logger;
        _logger.LogInformation("Json Serializer start working");
    }

    public string Serialize(object source)
    {
        LogSerizlie(source);

        var result = source.IsNotNull() ? JsonSerializer.Serialize(source, _options) : String.Empty;
        return result;
    }

    public string Serialize<TSource>(TSource source)
    {
        LogSerizlie(source);

        var result = source.IsNotNull() ? JsonSerializer.Serialize(source, _options) : String.Empty;
        return result;
    }

    public object? Deserialize(string source, Type type)
    {
        LogDeserizlie(source, type);

        var result = !source.IsNull() ? JsonSerializer.Deserialize(source, type) : default;
        return result;
    }

    public TOutput? Deserialize<TOutput>(string source)
    {
        LogDeserizlie(source, source.Type());

        var result = !source.IsNull() ? JsonSerializer.Deserialize<TOutput>(source, _options) : default;
        return result;
    }

    public void Dispose() => _logger.LogInformation("Json Serializer Stop working");

    private void LogSerizlie(object source) => _logger.LogTrace("Json Serializer Serialize with name {source}", source);

    private void LogDeserizlie(string source, Type type) => _logger
           .LogTrace("Json Serializer Deserialize with name {source} and type {type}", source, type);
}
