namespace Master.Utilities.Services.Implementation.Serializing;

using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Extentions;
using Abstraction.Serializing;

public class NewtonSoftSerialize : ISerialize
{
    private readonly ILogger<NewtonSoftSerialize> _logger;
    private JsonSerializerSettings _settings;

    public NewtonSoftSerialize(ILogger<NewtonSoftSerialize> logger)
    {
        _settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        _logger = logger;
        _logger.LogInformation("Newton Soft Serializer start working");
    }

    public string Serialize(object source)
    {
        LogSerizlie(source);

        var result = source.IsNotNull() ? JsonConvert.SerializeObject(source, _settings) : String.Empty;
        return result;
    }

    public string Serialize<TSource>(TSource source)
    {
        LogSerizlie(source);

        var result = source.IsNotNull() ? JsonConvert.SerializeObject(source, _settings) : String.Empty;
        return result;
    }

    public object? Deserialize(string source, Type type)
    {
        LogDeserizlie(source, type);

        var result = !source.IsNull() ? JsonConvert.DeserializeObject(source, _settings) : default;
        return result;
    }

    public TOutput? Deserialize<TOutput>(string source)
    {
        LogDeserizlie(source, source.Type());

        var result = !source.IsNull() ? JsonConvert.DeserializeObject<TOutput>(source, _settings) : default;
        return result;
    }

    public void Dispose() => _logger.LogInformation("Newton Soft Serializer Stop working");

    private void LogSerizlie(object source) => _logger.LogTrace("Newton Soft Serializer Serialize with name {source}", source);

    private void LogDeserizlie(string source, Type type) => _logger
           .LogTrace("Newton Soft Serializer Deserialize with name {source} and type {type}", source, type);
}