using IdentityHub.IdentityService.Application.Abstracts;
using Microsoft.Extensions.Options;
using Shared.Redis;
using StackExchange.Redis;

namespace IdentityHub.IdentityService.Infrastructure.Redis
{
    internal class RedisService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options) 
        : AbstractRedisService(connectionMultiplexer, options), IRedisService
    {
        
    }
}
