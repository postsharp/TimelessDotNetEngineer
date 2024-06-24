// Copyright (c) SharpCrafters s.r.o. See the LICENSE.md file in the root directory of this repository root for details.

using Metalama.Patterns.Caching.Backends.Redis;
using Metalama.Patterns.Caching.Building;
using StackExchange.Redis;

internal static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddDistributedMetalamaCaching( this IHostApplicationBuilder builder, string connectionName, string? keyPrefix = null )
    {
        builder.AddRedisClient( connectionName );

        builder.Services.AddMetalamaCaching( caching => caching.WithBackend( backend =>
        {
            var redisConnectionMultiplexer = backend.ServiceProvider!.GetRequiredService<IConnectionMultiplexer>();
            return backend.Redis( new( redisConnectionMultiplexer, keyPrefix ) );
        } ) );

        return builder;
    }
}