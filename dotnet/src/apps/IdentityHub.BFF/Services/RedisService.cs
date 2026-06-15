using Microsoft.Extensions.Options;
using Shared.Redis;
using StackExchange.Redis;

namespace IdentityHub.BFF.Services
{
    public class RedisService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options) 
        : AbstractRedisService(connectionMultiplexer, options)
    {
        
    }
}