namespace Master.Utilities.Services.Implementation.Caching.Wireup;

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Abstraction.Caching;

public static class CacheWireupExtentions
{
    public static IServiceCollection MemoryCacheWireup(IServiceCollection source) =>
        source
        .AddMemoryCache()
        .AddTransient<ICache, InMemoryCache>();

    #region sql server cache

    public static IServiceCollection DistributedSqlServerCacheWireup(IServiceCollection source, IConfiguration configuration, string configSectionName)
    {
        //source.AddTransient<ICache, DistributedSqlServerCache>();

        source.Configure<DistributedSqlServerCacheConfig>(configuration.GetSection(configSectionName));
        var configs = configuration.Get<DistributedSqlServerCacheConfig>();

        if (configs?.AutoCreateTable == true)
            CreateTable(configs);

        source
            .AddDistributedSqlServerCache(_ =>
            {
                _.ConnectionString = configs.ConnectionString;
                _.SchemaName = configs.Schema;
                _.TableName = configs.Table;
            })
            .AddTransient<ICache, DistributedSqlServerCache>();

        return source;
    }

    private static void CreateTable(DistributedSqlServerCacheConfig configs)
    {
        var table = configs.Table;
        var schema = configs.Schema;

        var command =
            $@"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                                                WHERE TABLE_SCHEMA={schema} AND TABLE_NAME={table})){"\n"}
            BEGIN {"\n"}
            CREATE TABLE [{schema}].[{table}]{"\n"}
            ({"\n"}
            [Id][nvarchar](449) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,{"\n"}
            [Value] [varbinary](max)NOT NULL,{"\n"}
            [ExpiresAtTime] [datetimeoffset](7) NOT NULL,{"\n"}
            [SlidingExpirationInSeconds] [bigint] NULL,{"\n"}
            [AbsoluteExpiration] [datetimeoffset](7) NULL,{"\n"}
            PRIMARY KEY(Id),{"\n"}
            INDEX Index_ExpiresAtTime NONCLUSTERED (ExpiresAtTime)){"\n"}
            ){"\n"}
            End";

        new SqlConnection(configs.ConnectionString)
            .Execute(command);
    }

    #endregion

    #region redis cache
    public static IServiceCollection DistributedRedisCacheWireup(IServiceCollection source, IConfiguration configuration, string configSectionName)
    {
        source.AddTransient<ICache, DistributedRedisCache>();
        source.Configure<DistributedRedisCacheConfig>(configuration.GetSection(configSectionName));
        var configs = configuration.Get<DistributedRedisCacheConfig>();

        source.AddStackExchangeRedisCache(_ =>
        {
            _.InstanceName = configs.InstanceName;
            _.Configuration = configs.Configuration;
        })
       /* .AddTransient<ICache, DistributedRedisCache>()*/;

        return source;
    }

    #endregion
}
