namespace Master.Utilities.Services.Implementation.Serializing.Wireup;

using Microsoft.Extensions.DependencyInjection;
using Abstraction.Serializing;

public static class SerializerWireupExtentions
{
    public static IServiceCollection NewtonSoftSerializerWireup(this IServiceCollection source) =>
         source.AddSingleton<ISerialize, NewtonSoftSerialize>();

    public static IServiceCollection MicrosoftSerializerWireup(this IServiceCollection source) =>
        source.AddSingleton<ISerialize, JsonSerialize>();

    public static IServiceCollection ExcelSerializerWireup(this IServiceCollection source) =>
       source.AddSingleton<IExcelSerialize, EPPlusExcelSerialize>();
}