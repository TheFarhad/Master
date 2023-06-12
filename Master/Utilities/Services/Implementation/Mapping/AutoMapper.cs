namespace Master.Utilities.Services.Implementation.Mapping;

using global::AutoMapper;
using Microsoft.Extensions.Logging;
using Abstraction.Mapping;

public class AutoMapper : IMap
{
    private readonly IMapper _mapper;
    private readonly ILogger<AutoMapper> _logger;

    public AutoMapper(IMapper mapper, ILogger<AutoMapper> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _logger.LogInformation("AutoMapper Start working");
    }

    public TOutput Map<TSource, TOutput>(TSource source)
    {
        _logger
            .LogTrace("AutoMapper  Map {source} To {destination} with data {sourcedata}",
                      typeof(TSource),
                      typeof(TOutput),
                      source);

        return _mapper.Map<TSource, TOutput>(source);
    }
}